# Studio Legale Avv. Maria Ambrosio - Sito Web

Un sito web professionale e minimal per uno studio legale freelance, sviluppato con .NET 8 e Bootstrap 5.

## Caratteristiche

- **Design Professionale**: Interfaccia pulita e moderna con colori bianco e rosso elegante
- **Responsive**: Completamente adattivo per dispositivi desktop, tablet e mobile
- **Form di Contatto Funzionale**: Invia email automaticamente all'avvocato
- **3 Pagine Principali**:
  - **Home**: Presentazione dell'avvocato con sezioni biografiche e professionali
  - **Competenze**: 8 aree di specializzazione legale con descrizioni dettagliate
  - **Contatti**: Modulo di contatto funzionale, informazioni di contatto

## Tecnologie Utilizzate

- .NET 8
- Bootstrap 5.3
- Font Awesome 6.0
- CSS Custom
- JavaScript vanilla
- MailKit per l'invio email

## Struttura del Progetto

```
SitoAvvocato/
??? Program.cs                 # Configurazione applicazione + API email
??? appsettings.json          # Configurazioni email
??? wwwroot/
?   ??? css/
?   ?   ??? style.css         # Stili personalizzati
?   ??? js/
?   ?   ??? main.js          # Funzionalità JavaScript
?   ??? images/
?   ?   ??? .gitkeep         # Directory per immagini
?   ??? index.html           # Pagina principale
?   ??? competenze.html      # Pagina competenze
?   ??? contatti.html        # Pagina contatti
```

## Funzionalità

### Navbar
- Logo a sinistra
- Menu di navigazione centrale (perfettamente centrato sulla pagina)
- Icone social (Email, LinkedIn, Instagram) all'estrema destra
- Design responsive con hamburger menu

### Form di Contatto
- **Invio Email Automatico**: Le richieste vengono inviate a `andrydeca2002@gmail.com`
- **Email di Conferma Cliente**: Viene inviata automaticamente una email di conferma elegante al cliente
- **Email Professionale**: Subject personalizzato con formattazione professionale
- **Validazione Completa**: Controlli client-side e server-side con feedback visivo
- **Popup Personalizzati**: Sistema di notifiche elegante
- **Campi**: Nome, Cognome, Email, Telefono, Area di interesse, Messaggio
- **Privacy e Newsletter**: Checkbox per consensi
- **Feedback Utente**: Messaggi di successo/errore con animazioni

### Pagine
- **Index**: Sezione hero, storia professionale, informazioni personali
- **Competenze**: 8 aree legali specializzate in formato card
- **Contatti**: Form di contatto funzionale, informazioni di contatto

### Stile
- Palette colori: Bianco primario, rosso elegante (#B22222)
- Typography: Georgia/Times New Roman per professionalità
- Cards con hover effects
- Bottoni personalizzati
- Layout pulito e spazioso

## Come Eseguire

1. Naviga nella directory del progetto
2. Esegui: `dotnet run`
3. Apri il browser su `https://localhost:5001` o `http://localhost:5000`

## Configurazione Email

### ? **Configurazione SMTP Gmail**

1. **Genera Password App Gmail:**
   - Vai su [myaccount.google.com](https://myaccount.google.com)
   - Sicurezza ? Verifica in due passaggi
   - Password per le app ? Genera nuova password

2. **Modifica `appsettings.json`:**
```json
{
  "EmailSettings": {
    "Username": "andrydeca2002@gmail.com",
    "Password": "LA_PASSWORD_GENERATA",
    "DevelopmentMode": false
  }
}
```

3. **Riavvia l'applicazione:**
```bash
dotnet run
```

### ?? **Formato Email Professionale**

Le email inviate hanno questo formato:

```
Subject: RICHIESTA DI RICONTATTO - [Area Legale] - [Nome Cognome]

===============================================================
                   STUDIO LEGALE AMBROSIO                    
                 RICHIESTA DI RICONTATTO                     
===============================================================

Gentile Avv. Ambrosio,

è pervenuta una nuova richiesta di consulenza dal sito web ufficiale.
Si prega di ricontattare il cliente entro 24 ore.

---------------------------------------------------------------
                  DETTAGLI DEL CLIENTE                      
---------------------------------------------------------------

NOMINATIVO:     Mario Rossi
EMAIL:          mario.rossi@email.com
TELEFONO:       +39 123 456 7890
AREA LEGALE:    Diritto di Famiglia

[... resto del messaggio ...]
```

### ?? **Modalità**
- **Sviluppo** (`DevelopmentMode: true`): Email simulate, solo log in console
- **Produzione** (`DevelopmentMode: false`): Email reali inviate via SMTP

## Sistema Email Automatico

Quando un cliente compila il form, il sistema invia automaticamente **2 email**:

#### 1. **Email Notifica all'Avvocato** (andrydeca2002@gmail.com)
```
Subject: RICHIESTA DI RICONTATTO - [Area Legale] - [Nome Cognome]

===============================================================
                   STUDIO LEGALE AMBROSIO                    
                 RICHIESTA DI RICONTATTO                     
===============================================================

Gentile Avv. Ambrosio,

è pervenuta una nuova richiesta di consulenza dal sito web ufficiale.
Si prega di ricontattare il cliente entro 24 ore.

---------------------------------------------------------------
                  DETTAGLI DEL CLIENTE                      
---------------------------------------------------------------

NOMINATIVO:     Mario Rossi
EMAIL:          mario.rossi@email.com
TELEFONO:       +39 123 456 7890
AREA LEGALE:    Diritto di Famiglia

[... resto del messaggio ...]
```

#### 2. **Email di Conferma al Cliente**
- **Formato**: HTML elegante con logo e branding
- **Subject**: "Conferma ricezione richiesta - Studio Legale Ambrosio"
- **Contenuto**: Messaggio personalizzato di ringraziamento e conferma
- **Design**: Template professionale responsive con colori aziendali

## Personalizzazione

Per personalizzare il sito:
1. Modifica i colori in `wwwroot/css/style.css` (variabili CSS in `:root`)
2. Aggiorna contenuti nei file HTML
3. Sostituisci `/images/MariaAmbrosio.jpg` con la foto reale
4. Modifica informazioni di contatto nelle pagine
5. Aggiorna l'email di destinazione in `appsettings.json`

## API Endpoints

- `GET /` - Pagina home
- `GET /competenze` - Pagina competenze  
- `GET /contatti` - Pagina contatti
- `POST /api/contact` - Invio form di contatto

## Note

- Il form di contatto è completamente funzionale e invia email a `andrydeca2002@gmail.com`
- Sostituire l'immagine placeholder con una foto professionale
- I link social vanno aggiornati con i profili reali
- Il sito è pronto per essere deployato su qualsiasi hosting che supporti .NET 8