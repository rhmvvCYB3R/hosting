document.addEventListener('DOMContentLoaded', () => {
  const restaurantList = document.getElementById('restaurant-list');

  if (!restaurantList) {
    console.error('Element for displaying restaurants not found');
    return;
  }

  // Load restaurants from the server
  async function loadRestaurants() {
    try {
      const res = await fetch('http://localhost:5137/api/Cafe');  // Make sure the URL is correct
      if (!res.ok) throw new Error(`Request error: ${res.status}`);

      const cafes = await res.json();
      renderRestaurants(cafes);
    } catch (err) {
      console.error('Error loading restaurants:', err);
      restaurantList.innerHTML = '<p>An error occurred while loading restaurants</p>';
    }
  }

  // Function to render restaurants
  function renderRestaurants(cafes) {
    if (cafes.length === 0) {
      restaurantList.innerHTML = '<p>No restaurants found</p>';
      return;
    }

    restaurantList.innerHTML = cafes.map(cafe => {
      const streetName = cafe.street || 'Unknown street';  // Default value if street is missing

      return `
        <div class="restaurant-card" data-id="${cafe.id}">
          <h3>MYK-${streetName}</h3>  <!-- Add "MYK-{street}" here -->
          <p>Rating: ${cafe.rating ? cafe.rating : 'Not specified'} ⭐</p>
          <p>Address: ${streetName}, ${cafe.city || 'Unknown city'}</p>
        </div>
      `;
    }).join('');

    // Add click handler to cards
    const cards = document.querySelectorAll('.restaurant-card');
    cards.forEach(card => {
      card.addEventListener('click', () => {
        const cafeId = card.getAttribute('data-id');
        window.location.href = `/restaurant/${cafeId}`;  // Redirect to restaurant page
      });
    });
  }

  loadRestaurants();  // Load restaurants on page load
});
