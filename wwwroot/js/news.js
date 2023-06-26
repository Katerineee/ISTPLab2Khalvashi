// Функция для обновления таблицы новостей
function updateNewsTable() {
    fetch('/api/News')
        .then(response => response.json())
        .then(newsList => {
            const tableBody = document.querySelector('#newsTable tbody');
            tableBody.innerHTML = '';

            newsList.forEach(news => {
                const row = document.createElement('tr');
                row.innerHTML = `
          <td>${news.title}</td>
          <td>${news.content}</td>
          <td>${news.createdAt}</td>
          <td>${news.category ? news.category.name : ''}</td>
          <td>
            <button onclick="editNews(${news.newsId})">Edit</button>
            <button onclick="deleteNews(${news.newsId})">Delete</button>
          </td>
        `;
                tableBody.appendChild(row);
            });
        });
}

// Функция для создания новости
function createNews(event) {
    event.preventDefault();

    const title = document.querySelector('#title').value;
    const content = document.querySelector('#content').value;
    const createdAt = document.querySelector('#createdAt').value;
    const category = document.querySelector('#category').value;

    const data = {
        Title: title,
        Content: content,
        CreatedAt: createdAt,
        Category: { Name: category }
    };

    fetch('/api/News', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    })
        .then(response => response.json())
        .then(createdNews => {
            console.log('Created news:', createdNews);
            updateNewsTable();
        })
        .catch(error => console.error('Error:', error));
}

// Функция для редактирования новости
function editNews(newsId) {
    fetch(`/api/News/${newsId}`)
        .then(response => response.json())
        .then(news => {
            document.querySelector('#editNewsId').value = newsId;
            document.querySelector('#editTitle').value = news.title;
            document.querySelector('#editContent').value = news.content;
            document.querySelector('#editCreatedAt').value = news.createdAt;
            document.querySelector('#editCategory').value = news.category ? news.category.name : '';

            const editNewsForm = document.querySelector('#editNewsForm');
            editNewsForm.removeEventListener('submit', handleEditNewsSubmit); // Удаление старого обработчика
            editNewsForm.addEventListener('submit', handleEditNewsSubmit); // Добавление нового обработчика
        });
}

// Функция для обработки отправки формы редактирования новости
function handleEditNewsSubmit(event) {
    event.preventDefault();

    const updatedNewsId = document.querySelector('#editNewsId').value;
    const updatedTitle = document.querySelector('#editTitle').value;
    const updatedContent = document.querySelector('#editContent').value;
    const updatedCreatedAt = document.querySelector('#editCreatedAt').value;
    const updatedCategory = document.querySelector('#editCategory').value;

    const updatedData = {
        Title: updatedTitle,
        Content: updatedContent,
        CreatedAt: updatedCreatedAt,
        Category: { Name: updatedCategory }
    };

    fetch(`/api/News/${updatedNewsId}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(updatedData)
    })
        .then(response => {
            if (response.ok) {
                console.log('News updated successfully');
                updateNewsTable();
            } else {
                return response.json();
            }
        })
        .then(data => {
            if (data && data.error) {
                console.error('Error updating news:', data.error);
            }
        })
        .catch(error => console.error('Error:', error));
}

// Функция для удаления новости
function deleteNews(event) {
    event.preventDefault();

    const newsId = document.querySelector('#deleteNewsId').value;

    fetch(`/api/News/${newsId}`, {
        method: 'DELETE'
    })
        .then(response => {
            if (response.ok) {
                console.log('News deleted successfully');
                updateNewsTable();
            } else {
                console.error('Error deleting news');
            }
        })
        .catch(error => console.error('Error:', error));
}

// Обновление таблицы новостей при загрузке страницы
window.addEventListener('load', function () {
    updateNewsTable();
});
