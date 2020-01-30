// infos:
// @version v0.13.3
// @author Edward Guilherme de Oliveira (Werefox#1236)
// embed: https://leovoel.github.io/embed-visualizer/

const Discord = require('discord.js')
const client = new Discord.Client()
const math = require('mathjs')
const fs = require('fs') //file system

// no startup
client.on('ready', () => {
    console.log("Connected as " + client.user.tag)
	// List servers the bot is connected to
    console.log("Servers:")
    client.guilds.forEach((guild) => {
        console.log(" - " + guild.name)
})
		
	// Set bot status to: "Playing with"
	options = ["with your waifu", "with lolis", "myself", "with your heart"];
	bot_choose = choice(options);
    client.user.setActivity(bot_choose); // essa linha muda
	
    // Alternatively, you can set the activity to any of the following:
    // PLAYING, STREAMING, LISTENING, WATCHING
    // For example:
    // client.user.setActivity("TV", {type: "WATCHING"})
	
	test_channel = "550892924016263168" // #Toymaking_Lab
	// Send something to a channel - 
	var generalChannel = client.channels.get(test_channel) // Replace with known channel ID
    generalChannel.send("I AM AWAKE!")
	// Send a Smug pic to a channel
	var generalChannel = client.channels.get(test_channel) // Replace with known channel ID
	// Provide a path to a local file
    const localFileAttachment = new Discord.Attachment('C:\\Users\\Bem Vindo\\Desktop\\Atalhos\\DISCORD BOTS\\Ageha Bot\\Pics\\Smug.jpg')
    generalChannel.send(localFileAttachment)
	
})

// bot repete se você coloca @Ageha
client.on('message', (receivedMessage) => {
    if (receivedMessage.author == client.user) { // Prevent bot from responding to its own messages
        return
    }
    
    if (receivedMessage.content.startsWith("|")) {
        processCommand(receivedMessage)
    }
	
    if (receivedMessage.content.includes(client.user.toString())) {
    // Send acknowledgement message
		receivedMessage.channel.send("Message received from " +
        receivedMessage.author.toString() + ": " + receivedMessage.content)
    }
})

// funcao para escolhas: rps, ask
function choice(choices){
    var index = Math.floor(Math.random() * choices.length);
    return choices[index];
}

// chamar funcao adicionando | (pipe)
function processCommand(receivedMessage) {
    let fullCommand = receivedMessage.content.substr(1) // Remove the leading exclamation mark
    let splitCommand = fullCommand.split(" ") // Split the message up in to pieces for each space
    let primaryCommand = splitCommand[0] // The first word directly after the exclamation is the command
    let arguments = splitCommand.slice(1) // All other words are arguments/parameters/options for the command

    console.log("Command received: " + primaryCommand)
    console.log("Arguments: " + arguments) // There may not be any arguments

    if (primaryCommand == "help") {
        helpCommand(arguments, receivedMessage)        // help
    } else if (primaryCommand == "multiply") {
        multiplyCommand(arguments, receivedMessage)    // multiply 
	} else if (primaryCommand == "ask") {
        askCommand(arguments, receivedMessage)         // ask
	} else if (primaryCommand == "summon") {
        summonCommand(arguments, receivedMessage)      // summon
	} else if (primaryCommand == "rps") {
        rpsCommand(arguments, receivedMessage)         // rps
	} else if (primaryCommand == "google") {
        googleCommand(arguments, receivedMessage)      // google
	} else if (primaryCommand == "list") {
        listCommand(arguments, receivedMessage)        // list
	} else if (primaryCommand == "choose") {
        chooseCommand(arguments, receivedMessage)      // choose
	} else if (primaryCommand == "smug") {
        smugCommand(arguments, receivedMessage)        // smug
    } else {
        receivedMessage.channel.send("I don't understand the command. Try `|help` or `|list`")
    }
}

// |help
function helpCommand(arguments, receivedMessage) {
    if (arguments.length > 0) {
        receivedMessage.channel.send("It looks like you might need help with " + arguments.toString().replace(/,/g, ' '))
    } else {
        receivedMessage.channel.send("I'm not sure what you need help with. Try `|help [topic]`")
    }
}

// |list
function listCommand(arguments, receivedMessage) {
	 receivedMessage.channel.send("```Commands list:\n\n|help\n|multiply\n|ask\n|summon\n|rps\n|google\n|choose\n|smug```")
}

// |multiply
function multiplyCommand(arguments, receivedMessage) {
    if (arguments.length < 2) {
        receivedMessage.channel.send("Not enough values to multiply. Try `!multiply 2 4 10` or `!multiply 5.2 7`")
        return
    }
    let product = 1 
    arguments.forEach((value) => {
        product = product * parseFloat(value)
    })
    receivedMessage.channel.send("The product of " + arguments + " multiplied together is: " + product.toString())
}

// |summon
function summonCommand(arguments, receivedMessage) {
	receivedMessage.channel.send("I AM AWAKE!")
	const localFileAttachment = new Discord.Attachment('C:\\Users\\Bem Vindo\\Desktop\\Atalhos\\DISCORD BOTS\\Ageha Bot\\Pics\\Smug.jpg')
    receivedMessage.channel.send(localFileAttachment)
}

// |ask
function askCommand(arguments, receivedMessage) {
	if (arguments.length > 0) {
		options = ["Yes", "No", "Maybe"];
		bot_choose = choice(options);
		receivedMessage.channel.send(bot_choose);
	} else {
		receivedMessage.channel.send("Try typing something, fag.")
	}
}

// |rps
function rpsCommand(arguments, receivedMessage) {
    if (arguments == ("Rock") || arguments == ("Paper") || arguments == ("Scissors")) {
		options = ["Rock", "Paper", "Scissors"];
		bot_choose = choice(options);
		// receivedMessage.channel.send(bot_choose);
		result = rps(arguments, bot_choose);
		// embed below
		const embed = new Discord.RichEmbed()
        .setTitle("Rock, Paper, Scissors")
        .setAuthor("Ageha", "https://cdn.discordapp.com/attachments/547151965902340117/551614683937898498/NewAgeha1024x1024.png")
        .setColor(0x00AE86)
		.setDescription(receivedMessage.author.toString() + " chooses: " + arguments + "!")
        .addField("I choose...", bot_choose + "!", false)
        //.setDescription("I choose " + bot_choose + "!")
        .addField("And result is...", result, true);
        receivedMessage.channel.send ({embed});
		//receivedMessage.channel.send (result);
	} else {
		receivedMessage.channel.send("Are you retard? Can't even say something right.");
	}
}

function rps(player_choose, bot){
    var result = "Nothing";

    if(player_choose == bot) {
        result = "Draw";
    } else {
        switch (bot){
            case "Rock":
                if(player_choose == "Paper"){
                    result = "You win...";
                }else{
                    result = "Too bad, I win";
                }
            break;
            case "Paper":
                if(player_choose == "Scissors") 
                    result = "You win...";
                else 
                    result = "Too bad, I win";
            break;
            case "Scissors":
                if(player_choose == "Rock") 
                    result = "You win...";
                else 
                    result = "Too bad, I win";
            break;
            deafult:
                result = "Baka onii-chan!";
            break;
        }
    }
    return result;
}

// |google
function googleCommand(arguments, receivedMessage) {
	if (arguments.length > 0) {
		receivedMessage.channel.send ("http://lmgtfy.com/?q=" + arguments.toString().replace(/,/g, '+'));
	} else {
		receivedMessage.channel.send("Try typing something, fag.")
	}
}

// |choose
function chooseCommand(arguments, receivedMessage) {
	if (arguments.length > 0) {
		bot_choose = choice(arguments);
		receivedMessage.channel.send(bot_choose);
	} else {
		receivedMessage.channel.send("Try typing something, fag.")
	}
}

// |smug
function smugCommand(arguments, receivedMessage) {
	var links = JSON.parse(fs.readFileSync('smug.json', 'utf8'));
	receivedMessage.channel.send(json_choice(links));
}

function json_choice(choices){
    var index = Math.floor(Math.random() * Object.keys(choices).length) + 1;
    return choices[index];
}

client.login("insert secret token here")