document.addEventListener('DOMContentLoaded', function () {
    const sendButton = document.getElementById('sendButton');
    const messageInput = document.getElementById('messageInput');
    const chatMessages = document.getElementById('chatMessages');
    const usersContainer = document.getElementById('usersContainer');
    const userList = document.getElementById('userList');

    // Lista de usuários simulada
    const users = ['Usuário 1', 'Usuário 2', 'Usuário 3', 'Usuário 4', 'Usuário 5'];

    // Adiciona os usuários à lista
    users.forEach(user => {
        const listItem = document.createElement('li');
        listItem.classList.add('list-group-item');
        listItem.dataset.user = user;
        listItem.textContent = user;
        userList.appendChild(listItem);
    });

    // Lista de conversas simulada
    const conversations = {};

    users.forEach(user => {
        conversations[user] = [];
    });

    usersContainer.addEventListener('click', function (event) {
        const selectedUser = event.target.dataset.user;
        if (selectedUser) {
            showConversation(selectedUser);
        }
    });

    sendButton.addEventListener('click', function () {
        const selectedUser = usersContainer.querySelector('.active').dataset.user;
        const messageText = messageInput.value.trim();
        if (messageText !== '') {
            conversations[selectedUser].push({ sender: 'Você', message: messageText });
            appendMessage('Você', messageText);
            messageInput.value = '';
        }
    });

    function showConversation(user) {
        const messages = conversations[user];
        chatMessages.innerHTML = '';
        messages.forEach((msg, index) => {
            appendMessage(msg.sender, msg.message, index);
        });
        markUserAsActive(user);
    }

    function appendMessage(sender, message, index) {
        const messageWrapper = document.createElement('li');
        messageWrapper.classList.add('message');
        messageWrapper.setAttribute('data-index', index);

        const senderElement = document.createElement('div');
        senderElement.classList.add('sender');
        senderElement.innerText = sender;
        messageWrapper.appendChild(senderElement);

        const messageBody = document.createElement('div');
        messageBody.classList.add('message-body');
        messageBody.innerText = message;
        messageWrapper.appendChild(messageBody);

        const optionsWrapper = document.createElement('div');
        optionsWrapper.classList.add('message-options');

        const editButton = document.createElement('button');
        editButton.classList.add('btn', 'btn-sm', 'btn-outline-secondary');
        editButton.innerText = 'Editar';
        editButton.onclick = function () {
            const editedMessage = prompt('Digite a mensagem editada:');
            if (editedMessage) {
                conversations[usersContainer.querySelector('.active').dataset.user][index].message = editedMessage;
                messageBody.innerText = editedMessage;
            }
        };
        optionsWrapper.appendChild(editButton);

        const deleteButton = document.createElement('button');
        deleteButton.classList.add('btn', 'btn-sm', 'btn-outline-danger');
        deleteButton.innerText = 'Excluir';
        deleteButton.onclick = function () {
            const confirmDelete = confirm('Tem certeza que deseja excluir esta mensagem?');
            if (confirmDelete) {
                conversations[usersContainer.querySelector('.active').dataset.user].splice(index, 1);
                messageWrapper.remove();
            }
        };
        optionsWrapper.appendChild(deleteButton);

        messageWrapper.appendChild(optionsWrapper);

        chatMessages.appendChild(messageWrapper);
        chatMessages.scrollTop = chatMessages.scrollHeight;
    }

    function markUserAsActive(user) {
        const activeUser = usersContainer.querySelector('.active');
        if (activeUser) {
            activeUser.classList.remove('active');
        }
        const userElement = usersContainer.querySelector(`[data-user="${user}"]`);
        userElement.classList.add('active');
    }
});