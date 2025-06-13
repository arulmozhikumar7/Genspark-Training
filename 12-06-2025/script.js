const apiUrl = 'https://jsonplaceholder.typicode.com/users/1';

function displayUser(user, method) {
  const output = document.getElementById("output");
  output.innerHTML = `
    <div class="user-card">
      <h2>${method}</h2>
      <p><strong>Name:</strong> ${user.name}</p>
      <p><strong>Username:</strong> ${user.username}</p>
      <p><strong>Email:</strong> ${user.email}</p>
      <p><strong>Phone:</strong> ${user.phone}</p>
      <p><strong>Company:</strong> ${user.company.name}</p>
      <p><strong>Website:</strong> <a href="http://${user.website}" target="_blank">${user.website}</a></p>
      <p><strong>Address:</strong> ${user.address.street}, ${user.address.city}</p>
    </div>
  `;
}

function displayError(error, method) {
  document.getElementById("output").innerHTML = `<p class="error">${method} Error: ${error.message}</p>`;
}

// Callback
function fetchUserWithCallback(url, callback) {
  fetch(url)
    .then(response => {
      if (!response.ok) {
        throw new Error("Fetch failed");
      }
      return response.json();
    })
    .then(data => callback(null, data))  
    .catch(err => callback(err, null));  
}

function getUserWithCallback() {
  fetchUserWithCallback(apiUrl, (err, user) => {
    if (err) return displayError(err, "Callback");
    displayUser(user, "Callback");
  });
}

// Promise
function fetchUserWithPromise(url) {
  return fetch(url).then(res => {
    if (!res.ok) throw new Error("Fetch error");
    return res.json();
  });
}

function getUserWithPromise() {
  fetchUserWithPromise(apiUrl)
    .then(user => displayUser(user, "Promise"))
    .catch(err => displayError(err, "Promise"));
}

// Async/Await
async function getUserWithAsyncAwait() {
  try {
    const res = await fetch(apiUrl);
    if (!res.ok) throw new Error("Fetch failed");
    const user = await res.json();
    displayUser(user, "Async/Await");
  } catch (err) {
    displayError(err, "Async/Await");
  }
}

// Reload
function reloadPage() {
  window.location.reload();
}