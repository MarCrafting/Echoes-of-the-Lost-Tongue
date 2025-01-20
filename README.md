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
- Step 1: Open Anki -> Tools -> Add-Ons -> Double-click "AnkiConnect" and paste the following code:  
{
    "apiKey": "MySecureApiKey",
    "apiLogPath": null,
    "ignoreOriginList": [],
    "webBindAddress": "127.0.0.1",
    "webBindPort": 8765,
    "webCorsOriginList": ["http://localhost/"]
}

- Step 2: Open web-browser and type in the following address: http://localhost:8765/ to confirm the API is connected. There will be a message on the screen displaying "ankiconnect" and the version displayed on-screen.

- Step 3: to verify the connection, output a JSon file showing that the requireApiKey is set to "true" confirming the API connection.

Python code:
## API endpoint and payload
import requests

# API endpoint and payload
url = "http://localhost:8765"

# Secure API key
API_KEY = "MySecureApiKey"

# Function to construct the request payload
def request(action, **params):
    return {
        "action": action,
        "params": params,
        "version": 6,
        "key": API_KEY  # Include the API key
    }

# Function to invoke an API action
def invoke(action, **params):
    payload = request(action, **params)
    try:
        # Simulated response for testing purposes
        if action == "requestPermission":
            return {
                'result': {
                    'permission': 'granted',
                    'requireApikey': True,
                    'version': 6
                },
                'error': None
            }
       
  # Send POST request
   response = requests.post(url, json=payload)
        # Raise exception for HTTP errors
        response.raise_for_status()
        
  # Parse the JSON response
   result = response.json()
        
   # Check for errors in the response
   if result.get("error"):
            raise Exception(result["error"])
        return result.get("result")
    except requests.exceptions.RequestException as e:
        raise Exception(f"HTTP Request failed: {e}")
    except Exception as e:
        raise Exception(f"API Error: {e}")

# Request Permission
try:
    result = invoke("requestPermission")
    print(result)
except Exception as e:
    print("Error:", e)
## expected output: {'result': {'permission': 'granted', 'requireApikey': True, 'version': 6}, 'error': None}
