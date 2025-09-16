# Script per configurare l'invio email reale
# Uso: .\setup_email.ps1

Write-Host "=== CONFIGURAZIONE EMAIL STUDIO LEGALE ===" -ForegroundColor Green
Write-Host ""

Write-Host "Per ricevere email reali all'indirizzo andrydeca2002@gmail.com:" -ForegroundColor Yellow
Write-Host ""

Write-Host "OPZIONE 1 - Configurazione Rapida (Gmail personale):" -ForegroundColor Cyan
Write-Host "1. Vai su https://myaccount.google.com"
Write-Host "2. Sicurezza -> Verifica in due passaggi"
Write-Host "3. Password per le app -> Genera nuova password"
Write-Host "4. Copia la password e modifica appsettings.json:"
Write-Host ""
Write-Host '   "Username": "andrydeca2002@gmail.com",' -ForegroundColor White
Write-Host '   "Password": "LA_PASSWORD_GENERATA",' -ForegroundColor White
Write-Host '   "DevelopmentMode": false' -ForegroundColor White
Write-Host ""

Write-Host "OPZIONE 2 - Test Veloce:" -ForegroundColor Cyan
Write-Host "Modifica solo DevelopmentMode in appsettings.json:"
Write-Host '   "DevelopmentMode": false' -ForegroundColor White
Write-Host "E inserisci le tue credenziali Gmail."
Write-Host ""

Write-Host "OPZIONE 3 - Servizio Email Professionale:" -ForegroundColor Cyan
Write-Host "Usa un servizio come SendGrid, Mailgun, o Amazon SES"
Write-Host ""

$response = Read-Host "Vuoi modificare automaticamente appsettings.json per test? (s/n)"

if ($response -eq "s" -or $response -eq "S") {
    $email = Read-Host "Inserisci il tuo email Gmail"
    $password = Read-Host "Inserisci la password app specifica" -AsSecureString
    $passwordText = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto([System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($password))
    
    $jsonContent = Get-Content "appsettings.json" | ConvertFrom-Json
    $jsonContent.EmailSettings.Username = $email
    $jsonContent.EmailSettings.Password = $passwordText
    $jsonContent.EmailSettings.DevelopmentMode = $false
    
    $jsonContent | ConvertTo-Json -Depth 10 | Set-Content "appsettings.json"
    
    Write-Host ""
    Write-Host "? Configurazione completata!" -ForegroundColor Green
    Write-Host "Ora riavvia l'applicazione con 'dotnet run' e testa il form." -ForegroundColor Green
    Write-Host "Le email verranno inviate realmente a andrydeca2002@gmail.com" -ForegroundColor Green
} else {
    Write-Host ""
    Write-Host "Configurazione manuale richiesta in appsettings.json" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "Dopo la configurazione, testa su: http://localhost:5000/test" -ForegroundColor Magenta