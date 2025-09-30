window.MostrarMenu = () => {
    document.getElementById("MenuHumburguer").style.display = "block"
    document.getElementById("main").style.overflow = "hidden";
}

window.OcultarMenu = () => {
    document.getElementById("MenuHumburguer").style.display = "none"
    document.getElementById("main").style.overflow = "auto";
}

window.MostrarEliminarCategoria = () => {
    document.getElementById("popEliminarCategoria").style.display = "flex";
}

window.OcultarEliminarCategoria = () => {
    document.getElementById("popEliminarCategoria").style.display = "none";
}

window.showOrHideModal = (modalId, isToShow) => {
    var modal = document.getElementById(modalId);
    
    if (!modal)
        return;
    
    if (isToShow) {
        modal.style.display = "flex";
    } else {
        modal.style.display = "none";
    }
}
