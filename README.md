## Blue Patcher Written with Mono C#

# State of project

- working under Windows as the original Visual Studio one
- working under Mac as intended


## Mac instructions

- Double-click the .command file provided with the package
- The path to blue.dll is a little odd:
- Filesystem/Applications/EVEOnline.app/Contents/Resources/EVEOnline.app/Contents/Resources/transgaming/c_drive/ProgramFilles/CCP/EVE/bin
- Select blue.dll and click on patch and you are done
- To start Eve Online navigate to the Applications folder, right-click on EVEOnline.app and click on Show Package Contents
- Navigate to Contents/Resources and there you find another EVEOnline.app - This is the one you should open and I recomend you drag it into the dock for easy access

### Make Eve Online point to EVEmu server

- Navigate to Applications/EVEOnline.app/Contents/Resources/EVEOnline.app/Contents/Resources/transgaming/c_drive/ProgramFilles/CCP/EVE/
- There you have to modify common.ini (change CryptoAPI to Placebo) and start.ini (change tranquility with localhost)
- Now the client is connecting to the EVEmu server

### Congratulations! Now you have a fully functioning EVEmu client
