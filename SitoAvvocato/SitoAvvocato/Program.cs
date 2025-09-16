using System.Text;
using System.Text.Json;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;

// Set default encoding to UTF-8
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
Console.OutputEncoding = Encoding.UTF8;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Configure UTF-8 encoding and static files with correct headers
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        if (ctx.File.Name.EndsWith(".html"))
        {
            ctx.Context.Response.Headers["Content-Type"] = "text/html; charset=utf-8";
        }
        else if (ctx.File.Name.EndsWith(".css"))
        {
            ctx.Context.Response.Headers["Content-Type"] = "text/css; charset=utf-8";
        }
        else if (ctx.File.Name.EndsWith(".js"))
        {
            ctx.Context.Response.Headers["Content-Type"] = "application/javascript; charset=utf-8";
        }
    }
});

// Route for home page
app.MapGet("/", async context => 
{
    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.SendFileAsync("wwwroot/index.html");
});

// Route for competenze page
app.MapGet("/competenze", async context => 
{
    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.SendFileAsync("wwwroot/competenze.html");
});

// Route for contatti page
app.MapGet("/contatti", async context => 
{
    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.SendFileAsync("wwwroot/contatti.html");
});

// Route for news page
app.MapGet("/news", async context => 
{
    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.SendFileAsync("wwwroot/news.html");
});

// Route for handling form submissions
app.MapPost("/api/contact", async (HttpContext context) =>
{
    Console.WriteLine("=== FORM SUBMISSION RECEIVED ===");
    Console.WriteLine($"Method: {context.Request.Method}");
    Console.WriteLine($"Content-Type: {context.Request.ContentType}");
    Console.WriteLine($"Time: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
    
    try
    {
        var formData = await context.Request.ReadFormAsync();
        Console.WriteLine($"Form data keys: {string.Join(", ", formData.Keys)}");
        
        var nome = formData["nome"].ToString();
        var cognome = formData["cognome"].ToString();
        var email = formData["email"].ToString();
        var telefono = formData["telefono"].ToString();
        var oggetto = formData["oggetto"].ToString();
        var messaggio = formData["messaggio"].ToString();
        var privacy = formData["privacy"].ToString();
        var newsletter = formData["newsletter"].ToString();

        Console.WriteLine($"Parsed data - Nome: {nome}, Cognome: {cognome}, Email: {email}");

        // Validate required fields
        if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(cognome) || 
            string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(oggetto) || 
            string.IsNullOrWhiteSpace(messaggio))
        {
            Console.WriteLine("Validation failed - missing required fields");
            context.Response.StatusCode = 400;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { 
                success = false, 
                message = "Tutti i campi obbligatori devono essere compilati." 
            }));
            return;
        }

        Console.WriteLine("Validation passed, creating email");

        // Get email settings from configuration
        var configuration = context.RequestServices.GetRequiredService<IConfiguration>();
        var recipientEmail = "andrydeca2002@gmail.com";

        // Create professional email message
        var message = new MimeMessage();
        
        // Professional sender with clear identification
        message.From.Add(new MailboxAddress("Studio Legale Ambrosio - Richiesta Contatto", configuration["EmailSettings:Username"] ?? "noreply@studiolegaleambrosio.it"));
        message.To.Add(new MailboxAddress("Avv. Maria Ambrosio", recipientEmail));
        
        // Professional subject line
        message.Subject = $"RICHIESTA DI RICONTATTO - {oggetto} - {nome} {cognome}";

        // Professional email body with better formatting (using standard ASCII characters)
        var bodyText = $@"
===============================================================
                   STUDIO LEGALE AMBROSIO                    
                 RICHIESTA DI RICONTATTO                     
===============================================================

Gentile Avv. Ambrosio,

e' pervenuta una nuova richiesta di consulenza dal sito web ufficiale.
Si prega di ricontattare il cliente entro 24 ore.

---------------------------------------------------------------
                  DETTAGLI DEL CLIENTE                      
---------------------------------------------------------------

NOMINATIVO:     {nome} {cognome}
EMAIL:          {email}
TELEFONO:       {(!string.IsNullOrWhiteSpace(telefono) ? telefono : "Non fornito")}
AREA LEGALE:    {oggetto}

---------------------------------------------------------------
                   RICHIESTA DEL CLIENTE                    
---------------------------------------------------------------

{messaggio}

---------------------------------------------------------------
                  CONSENSI E PREFERENZE                     
---------------------------------------------------------------

Consenso Privacy:    {(privacy == "on" ? "ACCORDATO" : "NON ACCORDATO")}
Newsletter:          {(newsletter == "on" ? "RICHIESTA" : "NON RICHIESTA")}

---------------------------------------------------------------
                 INFORMAZIONI TECNICHE                      
---------------------------------------------------------------

Data richiesta:      {DateTime.Now:dddd, dd MMMM yyyy}
Ora richiesta:       {DateTime.Now:HH:mm:ss}
Origine:             Sito web ufficiale
IP Address:          {context.Connection.RemoteIpAddress}

===============================================================
 Questa e' una notifica automatica del sistema del sito web  
       Per rispondere al cliente, utilizzare l'email:       
                        {email}                               
===============================================================

--
Studio Legale Avv. Maria Ambrosio
Via T.A. Cirillo n. 43, Boscoreale (NA)
Email: info@studiolegaleambrosio.it
Sistema automatico di notifiche v2.0
";

        message.Body = new TextPart("plain")
        {
            Text = bodyText
        };

        // Try to send email using Gmail SMTP
        try 
        {
            using var client = new SmtpClient();
            
            // Get email settings from configuration
            var developmentMode = configuration.GetValue<bool>("EmailSettings:DevelopmentMode", true);
            var smtpServer = configuration["EmailSettings:SmtpServer"] ?? "smtp.gmail.com";
            var smtpPort = configuration.GetValue<int>("EmailSettings:SmtpPort", 587);
            var username = configuration["EmailSettings:Username"];
            var password = configuration["EmailSettings:Password"];
            
            if (!developmentMode)
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    throw new InvalidOperationException("Username e Password SMTP devono essere configurati in appsettings.json");
                }
                
                Console.WriteLine($"Invio email tramite {smtpServer}:{smtpPort}");
                await client.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(username, password);
            }
            
            if (developmentMode)
            {
                // Development mode - just log to console
                Console.WriteLine($"=== [DEVELOPMENT MODE] EMAIL SIMULATA ALL'AVVOCATO ===");
                Console.WriteLine($"A: {recipientEmail}");
                Console.WriteLine($"Da: {email}");
                Console.WriteLine($"Oggetto: {message.Subject}");
                Console.WriteLine($"Messaggio:\n{bodyText}");
                Console.WriteLine($"========================================");
                
                Console.WriteLine($"=== [DEVELOPMENT MODE] EMAIL CONFERMA AL CLIENTE ===");
                Console.WriteLine($"A: {email}");
                Console.WriteLine($"Oggetto: Conferma ricezione richiesta - Studio Legale Ambrosio");
                Console.WriteLine($"Corpo: Email HTML di conferma");
                Console.WriteLine($"========================================");
                Console.WriteLine("Per ricevere email reali, configura SMTP in appsettings.json");
            }
            else
            {
                // Production mode - send both emails
                Console.WriteLine($"=== INVIO EMAIL NOTIFICA ALL'AVVOCATO ===");
                await client.SendAsync(message);
                Console.WriteLine($"Email notifica inviata a: {recipientEmail}");
                
                // Create confirmation email for client
                var confirmationMessage = new MimeMessage();
                confirmationMessage.From.Add(new MailboxAddress("Studio Legale Ambrosio", username));
                confirmationMessage.To.Add(new MailboxAddress($"{nome} {cognome}", email));
                confirmationMessage.Subject = "Conferma ricezione richiesta - Studio Legale Ambrosio";

                // Load HTML template from file and personalize it
                var templatePath = Path.Combine("wwwroot", "templates", "email-confirmation.html");
                
                if (!File.Exists(templatePath))
                {
                    Console.WriteLine($"=== ERROR: Template file not found at: {templatePath}");
                    throw new FileNotFoundException($"Email template not found at: {templatePath}");
                }
                
                var htmlTemplate = await File.ReadAllTextAsync(templatePath, Encoding.UTF8);
                
                Console.WriteLine($"=== DEBUG: Template loaded successfully");
                Console.WriteLine($"=== DEBUG: Template length: {htmlTemplate.Length} characters");
                
                // Personalize the template
                htmlTemplate = htmlTemplate.Replace("{NOME_CLIENTE}", nome);
                Console.WriteLine($"=== DEBUG: Template personalized with name: {nome}");

                // Create a multipart/alternative message with both HTML and text
                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = htmlTemplate;
                bodyBuilder.TextBody = $@"
Grazie per la tua fiducia

Caro/a {nome}, 

Abbiamo ricevuto la tua richiesta di consulenza per: {oggetto}

Lo Studio Legale Ambrosio ti contatterà al più presto all'indirizzo email: {email}
{(!string.IsNullOrWhiteSpace(telefono) ? $"Oppure al numero di telefono: {telefono}" : "")}

Cordiali saluti,
Studio Legale Avv. Maria Ambrosio
Via T.A. Cirillo n. 43, Boscoreale (NA)

© 2024 Studio Legale Avv. Maria Ambrosio. Tutti i diritti riservati.
";

                confirmationMessage.Body = bodyBuilder.ToMessageBody();

                Console.WriteLine($"=== INVIO EMAIL CONFERMA AL CLIENTE ===");
                await client.SendAsync(confirmationMessage);
                Console.WriteLine($"Email conferma inviata a: {email}");
                
                await client.DisconnectAsync(true);
                
                Console.WriteLine($"=== ENTRAMBE LE EMAIL INVIATE CORRETTAMENTE ===");
                Console.WriteLine($"Notifica avvocato: {recipientEmail}");
                Console.WriteLine($"Conferma cliente: {email}");
                Console.WriteLine($"============================");
            }
            
            // Return success response
            context.Response.ContentType = "application/json";
            var successResponse = JsonSerializer.Serialize(new { 
                success = true, 
                message = "Richiesta inviata con successo! Ti risponderemo entro 24 ore.",
                details = $"La tua richiesta per '{oggetto}' è stata ricevuta. Verrai contattato all'indirizzo {email}."
            });
            
            Console.WriteLine($"Sending success response: {successResponse}");
            await context.Response.WriteAsync(successResponse);
        }
        catch (Exception emailEx)
        {
            Console.WriteLine($"Errore nell'invio email: {emailEx.Message}");
            Console.WriteLine($"Stack trace: {emailEx.StackTrace}");
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { 
                success = false, 
                message = "Errore nell'invio dell'email. Il team tecnico è stato notificato." 
            }));
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Errore nella gestione del form: {ex.Message}");
        Console.WriteLine($"Stack trace: {ex.StackTrace}");
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(new { 
            success = false, 
            message = "Errore nella gestione della richiesta. Riprova più tardi." 
        }));
    }
});

app.Run();
