# App
This is a Document/File management app 

#Funcationality
List, upload, download and delete documents
Basic API connectivity test cases
Sample UI code to test the API

# Architecture decision
To handle multiple concurrent requests, I used thread lock to handle file deletion

# Demo
There is a index.html file to test the app. The files get uploaded to /Documents folder under the app's root folder.
Run using IIExpress as https on 44328 port

#Future improvemeents
Authentication/Authorization for users
Permission to handle file deletes
Save file information in the database
Log API call transactions
Add tests to handle file upload and download.
Setup Mock tests
Multiple concurrent requests, can be extended for other actions
Docker file is setup for Linux server
