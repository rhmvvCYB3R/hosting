document.addEventListener('DOMContentLoaded', () => {
  const openModalBtn = document.getElementById('open-modal');
  const modal = document.getElementById('modal');
  const closeBtn = document.getElementById('modal-close');
  const cancelBtn = document.getElementById('modal-cancel');
  const cafeForm = document.getElementById('cafe-form');
  const searchBtn = document.getElementById('search-btn');
  const searchInput = document.getElementById('search-restaurant');
  const restaurantList = document.getElementById('restaurant-list');
  const idField = document.getElementById('cafe-id-display'); // 🔥 Field to display ID

  let editingCafeId = null;

  if (!openModalBtn || !modal || !closeBtn || !cancelBtn || !cafeForm || !searchBtn || !searchInput || !restaurantList) {
    console.error('Not all required elements are found');
    return;
  }

  openModalBtn.addEventListener('click', () => {
    editingCafeId = null;
    cafeForm.reset();
    if (idField) idField.textContent = ''; // Clear ID
    modal.classList.add('open');
  });

  [closeBtn, cancelBtn].forEach(btn => {
    btn.addEventListener('click', () => {
      modal.classList.remove('open');
      cafeForm.reset();
      editingCafeId = null;
      if (idField) idField.textContent = '';
    });
  });

  document.addEventListener('keydown', (e) => {
    if (e.key === 'Escape') {
      modal.classList.remove('open');
      cafeForm.reset();
      editingCafeId = null;
      if (idField) idField.textContent = '';
    }
  });

  function convertToTimeString(time) {
    if (!time || typeof time !== 'string') return null;
    const [hours, minutes] = time.split(':');
    if (hours && minutes) return `${hours}:${minutes}:00`;
    return null;
  }

  function convertToTimeObject(time) {
    if (!time || typeof time !== 'string' || !time.includes(':')) return null;
    const [hours, minutes] = time.split(':');
    if (isNaN(hours) || isNaN(minutes)) return null;
    const totalMinutes = parseInt(hours, 10) * 60 + parseInt(minutes, 10);
    return { ticks: totalMinutes * 60000 };
  }

  cafeForm.addEventListener('submit', async (e) => {
    e.preventDefault();

    const formData = new FormData(cafeForm);
    const payload = {
      street: formData.get('cafe-street').trim(),
      city: formData.get('cafe-city').trim(),
      rating: parseFloat(formData.get('cafe-rating')) || 0,
      openingTime: convertToTimeString(formData.get('cafe-opening')),
      closingTime: convertToTimeString(formData.get('cafe-closing')),
      latitude: parseFloat(formData.get('cafe-lat')),
      longitude: parseFloat(formData.get('cafe-lon')),
    };

    const errors = [];
    if (!payload.city) errors.push('City');
    if (!payload.street) errors.push('Street');
    if (!payload.openingTime) errors.push('Opening time');
    if (!payload.closingTime) errors.push('Closing time');
    if (isNaN(payload.latitude)) errors.push('Latitude');
    if (isNaN(payload.longitude)) errors.push('Longitude');

    if (errors.length > 0) {
      alert(`Check the fields:\n${errors.join('\n')}`);
      return;
    }

    try {
      let response;
      if (editingCafeId) {
        // PUT request when editing
        response = await fetch(`http://localhost:5137/api/Cafe/Update`, {
          method: 'PUT',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(payload),
        });
      } else {
        // POST request when adding a new cafe
        response = await fetch('http://localhost:5137/api/Cafe', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(payload),
        });
      }

      const responseText = await response.text();
      console.log('Response Text:', responseText);

      if (response.ok) {
        alert(editingCafeId ? 'Cafe successfully updated!' : 'Cafe successfully added!');
        modal.classList.remove('open');
        cafeForm.reset();
        editingCafeId = null;
        if (idField) idField.textContent = '';
        searchBtn.click(); // Refresh search results
      } else {
        try {
          const errorData = JSON.parse(responseText);
          alert(`Error: ${errorData.title || 'Unknown error'}`);
        } catch (jsonErr) {
          console.error('Error parsing JSON:', jsonErr);
          alert('Error: failed to parse server response');
        }
      }
    } catch (err) {
      console.error('Network error:', err);
      alert('Network error: ' + err.message);
    }
  });

  searchBtn.addEventListener('click', async () => {
    const query = searchInput.value.trim();
    if (!query) {
      restaurantList.innerHTML = '<p>Please enter a street or ID to search</p>';
      return;
    }

    try {
      let cafes = [];

      // Search by ID
      if (!isNaN(query)) {
        const res = await fetch(`http://localhost:5137/api/Cafe/${query}`);
        if (res.ok) {
          const cafe = await res.json();
          cafes = [cafe];
        }
      }

      // If not found or not a number — search by street
      if (cafes.length === 0) {
        const res = await fetch(`http://localhost:5137/api/Cafe/by-street?street=${encodeURIComponent(query)}`);
        if (res.ok) {
          cafes = await res.json();
        } else if (res.status === 404) {
          restaurantList.innerHTML = '<p>No cafes found</p>';
          return;
        } else {
          throw new Error(`Request error: ${res.status}`);
        }
      }

      renderRestaurants(cafes);
    } catch (err) {
      console.error('Search error:', err);
      restaurantList.innerHTML = `<p style="color:red;">An error occurred during the search</p>`;
    }
  });

  function renderRestaurants(cafes) {
    if (cafes.length === 0) {
      restaurantList.innerHTML = '<p>No cafes found</p>';
      return;
    }

    restaurantList.innerHTML = cafes.map(cafe => `
      <div class="restaurant-card" data-id="${cafe.id}">
        <h3>${cafe.city} - ${cafe.street}</h3>
        <p>ID: ${cafe.id}</p>
        <p>Rating: ${cafe.rating}</p>
        <p>Opening time: ${cafe.openingTime}</p>
        <p>Closing time: ${cafe.closingTime}</p>
        <p>Latitude: ${cafe.latitude}</p>
        <p>Longitude: ${cafe.longitude}</p>
        <p>Created at: ${new Date(cafe.createdAt).toLocaleString()}</p>
        <button class="delete-btn">Delete</button>
        <button class="rate-btn">Rate</button>
        <button class="edit-btn">Edit</button>
      </div>
    `).join('');

    attachEventListeners();
  }

  function attachEventListeners() {
    document.querySelectorAll('.delete-btn').forEach(btn => {
      btn.addEventListener('click', async (e) => {
        const id = e.target.closest('.restaurant-card').dataset.id;
        if (confirm('Delete this cafe?')) {
          try {
            const res = await fetch(`http://localhost:5137/api/Cafe/${id}`, { method: 'DELETE' });
            if (res.ok) {
              alert('Cafe deleted');
              e.target.closest('.restaurant-card').remove();
            } else {
              alert('Error deleting cafe');
            }
          } catch (err) {
            console.error(err);
            alert('Network error while deleting');
          }
        }
      });
    });

    document.querySelectorAll('.rate-btn').forEach(btn => {
      btn.addEventListener('click', async (e) => {
        const id = e.target.closest('.restaurant-card').dataset.id;
        const rate = prompt('Enter a new rating (1 - 5):');
        const rating = parseFloat(rate);
        if (isNaN(rating) || rating < 1 || rating > 5) return alert('Invalid rating');

        try {
          const res = await fetch(`http://localhost:5137/api/Cafe/${id}/rate?rating=${rating}`, {
            method: 'POST',
          });

          if (res.ok) {
            alert('Rating updated');
            searchBtn.click();
          } else {
            alert('Error updating rating');
          }
        } catch (err) {
          console.error(err);
          alert('Network error');
        }
      });
    });

    document.querySelectorAll('.edit-btn').forEach(btn => {
      btn.addEventListener('click', async (e) => {
        const id = e.target.closest('.restaurant-card').dataset.id;
        try {
          const res = await fetch(`http://localhost:5137/api/Cafe/${id}`);
          if (!res.ok) throw new Error('Error fetching data');

          const cafe = await res.json();
          editingCafeId = cafe.id;

          if (!cafe.id) {
            alert('Error: Cafe ID missing');
            return;
          }

          if (idField) idField.textContent = `ID: ${cafe.id}`;

          cafeForm.elements['cafe-street'].value = cafe.street;
          cafeForm.elements['cafe-city'].value = cafe.city;
          cafeForm.elements['cafe-rating'].value = cafe.rating;
          cafeForm.elements['cafe-opening'].value = cafe.openingTime?.slice(0, 5);
          cafeForm.elements['cafe-closing'].value = cafe.closingTime?.slice(0, 5);
          cafeForm.elements['cafe-lat'].value = cafe.latitude;
          cafeForm.elements['cafe-lon'].value = cafe.longitude;

          modal.classList.add('open');
        } catch (err) {
          console.error(err);
          alert('Failed to load cafe data');
        }
      });
    });

  }
});
