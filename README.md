# Echoes-of-the-Lost-Tongue
 ![png](https://github.com/user-attachments/assets/9e8a4c47-b21e-418d-b217-fa14a5386ece)

> A language learning RPG providing the immersive environment best suited for learning new languages.

![GitHub last commit](https://img.shields.io/github/last-commit/MarCrafting/Echoes-of-the-Lost-Tongue)
![GitHub commit activity](https://img.shields.io/github/commit-activity/w/marcrafting/echoes-of-the-lost-tongue)
![GitHub contributors](https://img.shields.io/github/contributors/marcrafting/echoes-of-the-lost-tongue)

## Setup
1. Download Anki
   * Head over to [Anki's download page](aaps.ankiweb.net) and click "Download" to get Anki on your computer.
2. Install Anki-Connect
   * Open the ```Install Add-on``` dialog by selecting ```Tools```|```Add-ons```|```Get Add-ons...``` in Anki.
   * Input ```2055492159``` into the text box labeled ```Code``` and press the ```OK``` button to proceed.
   * Restart Anki when prompted to do so in order to complete the installation of Anki-Connect.
3. Configure Anki-Connect
   * Open the ```Configure 'AnkiConnect'``` window by selecting ```Tools```|```Add-ons```|```AnkiConnect``` in Anki.
   * Paste the following code below:
~~~
{
    "apiKey": "MySecureApiKey",
    "apiLogPath": null,
    "ignoreOriginList": [],
    "webBindAddress": "127.0.0.1",
    "webBindPort": 8765,
    "webCorsOriginList": ["http://localhost/"]
}
~~~

### Test Connection:
~~~
curl localhost:8765 -X POST -d "{\"action\":\"requestPermission\", \"version\":6}"
~~~
The result of the ```permission``` field should return ```granted``` to show a valid connection has been made.
Anki must remain running to hold a connection.

## Tech
### Developing environment
* UnityHub V3.11.0 (Editor Version 6000.0.29f1)
* Visual Studio Code(Version 1.96.3)/Community 22(Version 17.12.3)
### Resource
* Anki (Version 24.11)
### API
* AnkiConnect

## Setup
Copy the repository to your local machine.
Launch UnityHub and select "Add project from disk" item from the "Add" drop down menu.
Once loaded into the Unity project, you will be able to start the game.

## Project Status: Developing (Alpha)
