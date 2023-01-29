function agregarNuevaTareaAlListado() {
    tareaListadoViewModel.tareas.push(new tareaElementoListadoViewModel({ id: 0, titulo: '' }));

    $("[name=titulo-tarea]").last().focus();

}

async function manejarFocusoutTituloTarea(tarea) {
    var titulo = tarea.titulo();
    if (!titulo) {
        tareaListadoViewModel.tareas.pop();
        return;
    }

    let data = JSON.stringify(titulo);
   
    let respuesta = await fetch(`${urlTareas}/post`, {
        method: 'POST',
        data: data,
        headers: {
            'Content-Type': 'application/json'
        }
    });

    if (respuesta.ok) {
        const json = await respuesta.json();
        tarea.id(json.id);
    } else {
        manejarErrorApi(respuesta);
        return;
    }

    /*$.ajax({
        method: 'POST',
        url: `${urlTareas}/post`,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        data: titulo
    })
        .done(response => {
            Alert(response.msg, 'success', reloadPage)
            return true;
        })
        .fail(response => {
            console.log(response); 
            return false;
        })*/
}

async function obtenerTareas() {
    tareaListadoViewModel.cargando(true);

    const respuesta = await fetch(urlTareas, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    });

    if (!respuesta.ok) {
        manejarErrorApi(respuesta);
        return;
    }

    const json = await respuesta.json();
    tareaListadoViewModel.tareas([]);

    json.forEach(valor => {
        tareaListadoViewModel.tareas.push(new tareaElementoListadoViewModel(valor));
    });

    tareaListadoViewModel.cargando(false);

}

async function actualizarOrdenTareas() {
    const ids = obtenerIdsTareas();

    await enviarIdsTareasAlBackend(ids);

    const arregloOrdenado = tareaElementoListadoViewModel.tareas.sorted(function (a, b) {
        return ids.indexOf(a.id().toString()) - ids.indexOf(b.id().toString());
    });

    tareaListadoViewModel.tareas([]);
    tareaListadoViewModel.tareas(arregloOrdenado);
}

function obtenerIdsTareas() {
    const ids = $("[name=titulo-tarea]").map(function () {
        return $(this).attr("data-id");
    }).get();

    return ids;
}

async function enviarIdsTareasAlBackend(ids) {
    var data = JSON.stringify(ids);
    await fetch(`${urlTareas}/ordenar`, {
        method: 'POST',
        body: data,
        headers: {
            'Content-type': 'application/json'
        }
    });
}

async function manejarClickTarea(tarea) {
    if(tarea.esNuevo()){
        return; 
    }

    const respuesta = await fetch(`${urlTareas}/${tarea.id()}`,{
        method: 'GET',
        headers: {
            'Content-type': 'application/json'
        }
    })

    if (!respuesta.ok) {
        manejarErrorApi(respuesta);
        return;
    }

    const json = await respuesta.json();
    

    tareaEditarVM.id = json.id; 
    tareaEditarVM.titulo(json.titulo);
    tareaEditarVM.descripcion(json.descripcion);

    modalEditarTareaBootstrap.show();
}

async function manejarCambioEditarTarea() {
    const obj = {
        id: tareaEditarVM.id,
        titulo: tareaEditarVM.titulo(),
        descripcion: tareaEditarVM.descripcion()
    };

    if (!obj.titulo) {
        return;
    }

    await editarTareaCompleta(obj);

    const indice = tareaListadoViewModel.tareas().findIndex(t => t.id() === obj.id); 
    const tarea = tareaListadoViewModel.tareas()[indice]; 
    tarea.titulo(obj.titulo);
}

async function editarTareaCompleta(tarea) {
    const data = JSON.stringify(tarea);

    const respuesta = await fetch(`${urlTareas}/${tarea.id}`, {
        method: 'PUT',
        body: data,
        headers: {
            'Content-type': 'application/json'
        }
    });

    if (!respuesta.ok) { //SI LA RESPUESTA FALLA 
        manejarErrorApi(respuesta);
        throw "error"; //DETIENE CUALQUIER COSA QUE SE ESTE CORRIENDO EN LA APP
    }
}

function intentarBorrarTarea(tarea) {
    modalEditarTareaBootstrap.hide();

    confirmarAccion({
        callBackAceptar: () => {
            borrarTarea(tarea); 
        },
        callBackCancelar: () => {
            modalEditarTareaBootstrap.show();
        },
        titulo: `¿Desea borrar la tarea ${tarea.titulo}?`
    })
}

async function borrarTarea(tarea) {
    const idTarea = tarea.id; 

    const respuesta = await fetch(`${urlTareas}/${idTarea}`, {
        method: 'DELETE',
        headers: {
            'Content-type': 'application/json'
        }
    });

    if (respuesta.ok) {
        const indice = obtenerIndiceTareaEnEdicion();
        tareaElementoListadoViewModel.tareas.splice(indice, 1);
    }
}

function obtenerIndiceTareaEnEdicion() {
    return tareaListadoViewModel.tareas().findIndex(t => t.id() == tareaEditarVM.id);
}

$(function () {
    $("#reordenable").sortable({
        axis: 'y',
        stop: async function () {
            await actualizarOrdenTareas();
        }
    })
})