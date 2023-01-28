async function manejarErrorApi(respuesta) {
    let mensajeError = ''; 

    if (respuesta.status === 400) {
        mensajeError = await respuesta.text();
    } else if (respuesta.status === 404) {
        mensajeError = recursoNoEncontrado;
    } else {
        mensajeError = errorInesperado
    }

    mostrarMensajeError(mensajeError);
}

function mostrarMensajeError(mensaje) {
    swal.fire({
        icon: 'error',
        title: 'Error...',
        text: mensaje
    });
}


//Un callBack es una función que yo le paso a otra función en un contexto determinado
function confirmarAccion({ callBackAceptar, callBackCancelar, titulo }) {
    swal.fire({
        icon: 'warning',
        title: titulo || '¿Realmente deseas hacer esto?',
        showCancelButton: true, 
        confirmButtonColor: '#3085d',
        cancelButtonColor: 'd33',
        confirmButtonText: 'Sí',
        focusConfirm: true
    }).then((resultado) => {
        if (resultado.isConfirmed) {
            callBackAceptar();
        } else if (callBackCancelar) {
            // EL USUARIO HA PRESIONADO EL BOTÓN DE CANCELAR
            callBackCancelar();
        }
    });
}