function left() {}
function right() {}
function up() {}
function down() {}

function tap() {}
function press() {}

// Function to make a POST request
async function makePostRequest(url, data) {
    const response = await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    });

    const result = await response.json();
    return result;
}

// Function to speak text
function speakText(text) {
    
    const speech = new SpeechSynthesisUtterance(text); // Create a speech instance
    speech.lang = 'en-US'; // Set the language
    speech.pitch = 1; // Set the pitch (0.5 to 2)
    speech.rate = .8; // Set the rate (speed, 0.1 to 10)
    speech.volume = 1; // Set the volume (0 to 1)

    window.speechSynthesis.speak(speech); // Trigger the speech synthesis
}

// Function to handle incoming messages
function handleMessage(message) {
    switch (message) {
        case "left":
            left();
            console.log("Action: Move left");
            break;
        case "right":
            right();
            console.log("Action: Move right");
            break;
        case "up":
            up();
            console.log("Action: Move up");
            break;
        case "down":
            down();
            console.log("Action: Move down");
            break;
        case "tap":
            tap();
            console.log("Action: Move down");
            break;
        case "press":
            press();
            console.log("Action: Move down");
            break;
        default:
            console.log("Unknown action:", message);
    }
}

// Listen for messages from the parent
window.addEventListener('message', (event) => {
    const message = event.data;
    
    // Handle the message
    handleMessage(message);
});

async function getNavigationInstructions(destination) {
    try {
        const requestBody = { destination: destination };

        const response = await fetch('/navigate', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(requestBody)
        });

        const data = await response.json();
        console.log('Navigation Instructions:', data.steps);
        return data.steps;
    } catch (error) {
        console.error('Error getting navigation instructions:', error);
    }
}

async function getFavoriteDestinations() {
    try {
        const response = await fetch('/favorite-destinations', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        const data = await response.json();
        console.log('Favorite Destinations:', data.favorites);
        return data.favorites;
    } catch (error) {
        console.error('Error getting favorite destinations:', error);
    }
}

async function setSetup(newSetup) {
    try {
        const response = await fetch('/set-setup', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(newSetup) // Send new setup as JSON
        });

        if (response.ok) {
            const data = await response.json();
            console.log('Setup updated:', data.status);
        } else {
            const errorData = await response.json();
            console.error('Error:', errorData.error);
        }
    } catch (error) {
        console.error('Error setting up:', error);
    }
}

async function getSetup() {
    try {
        const response = await fetch('/setup', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        });
        const data = await response.json();
        console.log('Setup Information:', data.items);
        return data.items;
    } catch (error) {
        console.error('Error fetching setup:', error);
    }
}

