# Echoes-of-the-Lost-Tongue
 ![png](https://github.com/user-attachments/assets/9e8a4c47-b21e-418d-b217-fa14a5386ece)

> A language learning RPG providing the immersive environment best suited for learning new languages.

![GitHub last commit](https://img.shields.io/github/last-commit/MarCrafting/Echoes-of-the-Lost-Tongue)
![GitHub commit activity](https://img.shields.io/github/commit-activity/w/marcrafting/echoes-of-the-lost-tongue)
![GitHub contributors](https://img.shields.io/github/contributors/marcrafting/echoes-of-the-lost-tongue)

## Tech
### Developing environment
* UnityHub V3.11.0 (Editor Version 6000.0.29f1)
* Visual Studio Code(Version 1.96.3)/Community 22(Version 17.12.3)
### Resource
* Anki
### API
* AnkiConnect

## Setup
Copy the repository to your local machine.
Launch UnityHub and select "Add project from disk" item from the "Add" drop down menu.
Once loaded into the Unity project, you will be able to start the game.

## Project Status: Developing (Alpha)

## Code Examples: Particularly good for referencing API endpoints:
Step 1: Open Anki -> Tools -> Add-Ons -> Double-click "AnkiConnect" and paste the following code:  
{
    "apiKey": "MySecureApiKey",
    "apiLogPath": null,
    "ignoreOriginList": [],
    "webBindAddress": "127.0.0.1",
    "webBindPort": 8765,
    "webCorsOriginList": ["http://localhost/"]
}

Step 2: Open web-browser and type in the following address: http://localhost:8765/ to confirm the API is connected. There will be a message on the screen displaying "ankiconnect" and the version displayed on-screen.

Step 3: to verify the connection, output a JSon file showing that the requireApiKey is set to "true" confirming the API connection.

Python code:
## API endpoint and payload
url = "http://localhost:8765/"
payload = {
    "action": "requestPermission",
    "version": 6
}

## Send POST request
response = requests.post(url, json=payload)

Print the response
print(response.json())

API_KEY = "MySecureApiKey"  # Replace with your actual API key from AnkiConnect

def request(action, **params):
    return {
        'action': action,
        'params': params,
        'version': 6,
        'key': API_KEY  # Include the API key
    }
## expected output: {'result': {'permission': 'granted', 'requireApikey': True, 'version': 6}, 'error': None}
