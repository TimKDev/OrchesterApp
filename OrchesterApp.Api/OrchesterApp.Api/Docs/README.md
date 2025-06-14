## Developing
- Start Backend with `docker compose up`
- Start Frontend separately with `ionic serve`

## First Time Setup
When starting the app for the first time use the following steps to create the initial first user:
1. Start the application.
2. Execute the scripts: `SQL/addInstruments.sql` and `SQL/addMitglieder.sql`.
3. Then register your own user with the registration link `http://localhost:8100/auth/registration?registrationKey={yourRegistrationKey}&email={yourEmail}`
4. Execute the script: `SQL/addTimKempkensAsAdmin.sql`.
