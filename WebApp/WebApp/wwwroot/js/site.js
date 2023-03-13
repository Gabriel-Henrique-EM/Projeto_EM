const exampleModal = document.getElementById('confirmDelete')
exampleModal.addEventListener('show.bs.modal', event => {

    const button = event.relatedTarget
    const matricula = button.getAttribute('data-bs-whatever')
    const buttonDelet = confirmDelete.querySelector('#buttonDelete')
    buttonDelet.setAttribute('href', "/aluno/ConfirmarDeletar/" + matricula)
})