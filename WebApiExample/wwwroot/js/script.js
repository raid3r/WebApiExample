﻿var taskList = []
var isAuthorized = false

const taskTableBody = document.querySelector("table#task-list tbody")

const authorizedContent = document.getElementById("authorized")
const notAuthorizedContent = document.getElementById("not-authorized")


function renderTaskList() {
    const html = taskList.map(task => `
                <tr data-id="${task.id}">
                    <td>
                    <input class="is-completed" type="checkbox" ${task.isComplete ? "checked" : ""}>
                    </td>
                    <td>
                        <h4>${task.title}</h4>
                        <p class="text-small">${task.description}</p>
                    </td>
                    <td>
                        <button class="btn btn-danger task-delete">X</button>
                    </td>
                </tr>

    `).join("")
    taskTableBody.innerHTML = html

    document.querySelectorAll("input.is-completed").forEach(el => {
        el.addEventListener("change", e => {
            const id = Number(e.target.closest("[data-id]").getAttribute("data-id"))
            const task = taskList.find(x => x.id === id)
            task.isComplete = e.target.checked

            fetch(`api/v1/todo-list/${id}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + localStorage.getItem('token')
                },
                body: JSON.stringify(task)
            }).then(r => r.json()).then(() => {
                renderTaskList()
            })
        })
    })

    document.querySelectorAll("button.task-delete").forEach(el => {
        el.addEventListener("click", e => {
            const id = Number(e.target.closest("[data-id]").getAttribute("data-id"))
            fetch(`api/v1/todo-list/${id}`, {
                headers: {
                    'Authorization': 'Bearer ' + localStorage.getItem('token')
                },
                method: 'DELETE',
            }).then(r => r.json()).then(() => {
                taskList = taskList.filter(x => x.id !== id)
                renderTaskList()
            })
        })
    })
}

document.querySelector("form#login").addEventListener("submit", function (e) {

    e.preventDefault()
    const formData = new FormData(e.target)

    const loginForm = {
        email: formData.get("email"),
        password: formData.get("password"),
    }

    fetch("api/v1/auth/login", {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(loginForm)
    }).then(r => r.json())
        .then(data => {
            if (data.success) {
                localStorage.setItem('token', data.token)
                handleAuthorization()
                loadTasks()
                renderTaskList()
            }
        
    }).catch(err => {
        console.error(err)
    })
})

document.querySelector("form#add-task").addEventListener("submit", function (e) {

    e.preventDefault()
    const formData = new FormData(e.target)

    function formatDate(date) {
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const day = String(date.getDate()).padStart(2, '0');
        const formattedDate = `${year}-${month}-${day}`;
        return formattedDate
    }


    const task = {
        title: formData.get("title"),
        description: formData.get("description"),
        isComplete: false,
        createdAt: formatDate(new Date())
    }

    fetch("api/v1/todo-list", {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + localStorage.getItem('token')
        },
        body: JSON.stringify(task)
    }).then(r => r.json()).then(data => {
        taskList.push(data)
        renderTaskList()
    })
})

function handleAuthorization() {
    isAuthorized = localStorage.getItem("token") !== null
    console.log(localStorage.getItem("token"))
    console.log('Authorized', isAuthorized)
    notAuthorizedContent.style.display = isAuthorized ? "none" : "block"
    authorizedContent.style.display = isAuthorized ? "block" : "none"
}

function loadTasks() {
    fetch("api/v1/todo-list", {
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('token')
        },
    }).then(r => r.json()).then(data => {
        taskList = data
        renderTaskList()
    })
}

function init() {

    handleAuthorization()
    if (isAuthorized) {
        loadTasks()
    }

}

init()