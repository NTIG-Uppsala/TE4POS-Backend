async function getData() {
  try {
    const res = await fetch("api/v1/products");
    if (!res.ok) throw new Error("Failed to fetch");

    const products = await res.json();
    const table = document.getElementById("table-body");

    products.forEach(product => {
      table.innerHTML += `
        <tr>
          <td>${product.name}</td>
          <td>${product.price}</td>
          <td>${product.category}</td>
          <td>${product.stock}</td>
        </tr>
      `;
    });
  } catch (err) {
    console.error(err);
  }
}

getData();
