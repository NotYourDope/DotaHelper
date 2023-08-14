The application is only in Russian.

This is a Dota 2 gaming assistant called "DotaHelper". It is my university degree project.
An application should be connected to any database (in my case, it was MS SQL Server) to get information about users and heroes from there.
It is a prototype for a larger and more functional application. It is not connected to any Dota 2 files at any point. Not a cheating program.
No MVVM.

-- Registration process --
1. A minimum of six symbols for the username and password. Also checks for the same existing username in the DB.
2. User's password is being saved in DB in double-hash format.

-- Login Process --
1. Looks for an existing account in the database using the username and password entered.

-- Login / No login variant -- 
1. If the user wants not to log in, there is a button for that. In the main window, there will be a button to authorize.
2. If the user is logged in, his username will be displayed in the main window.

-- Main window --

  -- Drafting assistant --
  1. Depending on the chosen heroes for enemies and allies, tips will be displayed according to information taken from the DB.

  -- Timing assistant --
  1. Starts a global timer that is supposed to be an approximate analog for the in-game timer to control runes. 
  2. Audio alerts for runes events (.mp4 files)
  3. Roshan and the tormentor's timing assistant.
