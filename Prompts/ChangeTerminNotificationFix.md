I want you to work on the following two tasks concerning ChangeTerminNotifications:

## Extend Emails
Please add all information HasTreffpunkt, HasDokument and all other flags that are currently not added to the mail notification to the email. Also add the information about the termin (Name and Starttime) analogous to the notification in the frontend.

## Trigger for Email sending should be user 
I want that everytime a user makes a change to a Termin in the frontend. The user should be asked via a short dialog "Sollen alle Mitglieder per E-Mail über die Änderung benachrichtigt werden?". The result from dialog should be send as a boolian to the backend TerminUpdate Endpoint and then decide whether the notification is only sended to the frontend (always) or also via email. 

Please also implement the requirement completely analgous to previous code and try using as little code as possible. No comments.
