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

## Refrencing API Endpoints:
- Step 1: Configure AnkiConnect API
Open Anki and follow these steps:

1. Go to Tools > Add-Ons.
2. Double-click "AnkiConnect".
3. Paste the following configuration code into the editor:
{
    "apiKey": "MySecureApiKey",
    "apiLogPath": null,
    "ignoreOriginList": [],
    "webBindAddress": "127.0.0.1",
    "webBindPort": 8765,
    "webCorsOriginList": ["http://localhost/"]
}

- Step 2: Verify API Connection
Open your web browser and navigate to:

http://localhost:8765/
The following message "ankiconnect" and the version number should be displayed. This confirms that the API is running.

- Step 3: Verify Connection via Code
To ensure the API is functional, verify that requireApiKey is set to "true" by sending a POST request.
Here’s how you can programmatically interact with the AnkiConnect API using Python:

import requests

# API endpoint and your secure API key
url = "http://localhost:8765"
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
        # Send POST request to the API
        response = requests.post(url, json=payload)
        response.raise_for_status()  # Raise HTTP errors
        result = response.json()

   # Check for API-specific errors
   if result.get("error"):
            raise Exception(result["error"])
        return result.get("result")
    except requests.exceptions.RequestException as e:
        raise Exception(f"HTTP Request failed: {e}")
    except Exception as e:
        raise Exception(f"API Error: {e}")

# Example: Request permission from the API
try:
    result = invoke("requestPermission")
    print(result)
except Exception as e:
    print("Error:", e)
Expected Output:
json
Copy
Edit
{
    "result": {
        "permission": "granted",
        "requireApiKey": true,
        "version": 6
    },
    "error": null
}
