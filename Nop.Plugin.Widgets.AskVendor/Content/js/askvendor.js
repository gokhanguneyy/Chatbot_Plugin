function sendMessage(modelName) {

  var userMessage = document.getElementById("user-message").value;
  if (userMessage.trim() !== "") { // Kullanıcı mesajı boş değilse devam et
    var question = "Now I will ask you a question. The question and answer should be about " + modelName + " If the question is not about " + modelName + " , give a warning message." +
      "Question: " + userMessage;

    //var question ="Question: " + userMessage + ". Question is for " + modelName + " If the Question is not about " + modelName + " , give a warning message.";
    addMessageToChat(userMessage, "received");
    $.get('/Gpt/SendNewRequest', { request: question }, function (data, textStatus, jqXHR)){
      console.log(question);
        addMessageToChat(data, "sent");
      };
    document.getElementById("user-message").value = ""; // Mesaj kutusunu temizle 
  }
}

function addMessageToChat(message, type) {
  var chatBox = document.querySelector(".chat-box");
  var messageDiv = document.createElement("div");
  messageDiv.classList.add("message");
  messageDiv.classList.add(type);
  messageDiv.textContent = message;
  chatBox.appendChild(messageDiv);
}