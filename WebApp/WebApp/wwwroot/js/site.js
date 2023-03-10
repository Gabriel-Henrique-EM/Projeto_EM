const exampleModal = document.getElementById('confirmDelete')
exampleModal.addEventListener('show.bs.modal', event => {

    const button = event.relatedTarget
    const recipient = button.getAttribute('data-bs-whatever')
    const buttonDelet = confirmDelete.querySelector('#buttonDelete')
    buttonDelet.setAttribute('href', "/aluno/deletar/" + recipient)
})