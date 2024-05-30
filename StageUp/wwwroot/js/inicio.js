document.getElementById('fileBtn').addEventListener('click', function () {
    document.getElementById('fileInput').click();
});

document.getElementById('fileBtn2').addEventListener('click', function () {
    document.getElementById('fileInput').click();
});



document.addEventListener('DOMContentLoaded', function () {
    const inputText = document.getElementById('InputText');
    const inputBtn = document.getElementById('InputBtn');
    const commentPost = document.getElementById('commentPost');
    const modal = new bootstrap.Modal(document.getElementById('exampleModal'));
    const saveEditBtn = document.getElementById('saveEditBtn');
    const modalCloseBtn = document.getElementById('modalClose');
    let editedComment = null;

    inputBtn.addEventListener('click', function () {
        sendComment();
    });

    inputText.addEventListener('keypress', function (event) {
        if (event.key === 'Enter') {
            sendComment();
        }
    });

    function sendComment() {
        const commentText = inputText.value.trim();

        if (commentText !== '') {
            if (editedComment) {
                // Atualizar o comentário editado
                editedComment.querySelector('.comment-content').textContent = commentText;
                editedComment = null;
            } else {
                // Criar um novo elemento div para representar o comentário
                const newComment = document.createElement('div');
                newComment.classList.add('comment');

                // Criar uma div para o conteúdo do comentário
                const commentContent = document.createElement('div');
                commentContent.classList.add('comment-content');
                commentContent.textContent = commentText;

                // Criar uma div para as ações do comentário (ex: botões de editar/excluir)
                const commentActions = document.createElement('div');
                commentActions.classList.add('comment-actions');

                // Botão Editar
                const editBtn = document.createElement('button');
                editBtn.classList.add('btn', 'btn-primary');
                editBtn.textContent = 'Editar';
                editBtn.addEventListener('click', function () {
                    // Preencher o modal com o texto do comentário
                    inputText.value = commentText;
                    modal.show();
                    editedComment = newComment;
                });
                commentActions.appendChild(editBtn);

                // Botão Excluir
                const deleteBtn = document.createElement('button');
                deleteBtn.classList.add('btn', 'btn-danger');
                deleteBtn.textContent = 'Excluir';
                deleteBtn.addEventListener('click', function () {
                    // Remover o comentário da lista
                    commentPost.removeChild(newComment);
                    modal.hide(); // Fechar o modal ao excluir
                });
                commentActions.appendChild(deleteBtn);

                // Adicionar a div de conteúdo e ações ao novo elemento de comentário
                newComment.appendChild(commentContent);
                newComment.appendChild(commentActions);

                // Adicionar o novo elemento de comentário à div commentPost
                commentPost.appendChild(newComment);

                // Fechar o modal ao enviar o comentário
                modal.hide();

                // Limpar o campo de texto após adicionar o comentário
                inputText.value = '';
            }
        } else {
            alert('Por favor, digite um comentário antes de enviar.');
        }
    }

    // Botão Salvar no modal de edição
    saveEditBtn.addEventListener('click', function () {
        const editedText = inputText.value.trim();

        if (editedText !== '') {
            // Atualizar o texto do comentário
            editedComment.querySelector('.comment-content').textContent = editedText;
            editedComment = null;

            // Fechar o modal ao salvar a edição
            modal.hide();

            // Limpar o campo de texto do modal
            inputText.value = '';
        } else {
            alert('Por favor, digite um texto válido para salvar a edição.');
        }
    });

    // Botão Fechar modal
    modalCloseBtn.addEventListener('click', function () {
        inputText.value = ''; // Limpar o campo de texto do modal
        modal.hide();
    });
});