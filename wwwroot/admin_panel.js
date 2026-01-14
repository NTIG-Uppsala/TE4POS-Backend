async function getData() {
  try {
    const res = await fetch("/api/v1/products");
    if (!res.ok) throw new Error("Failed to fetch");

    const products = await res.json();
    const table = document.getElementById("table-body");

    products.forEach(product => {
      table.innerHTML += `
        <tr data-id="${product.id}">
          <td class="name">${product.name}</td>
          <td class="price">${product.price}</td>
          <td class="category">${product.category}</td>
          <td class="stock">${product.stock}</td>
          <td><button onclick="editProduct(this)">Redigera</button></td>
          <td><button class="delete-btn" data-id="${product.id}">Ta bort</button></td>
        </tr>
      `;
    });
  } catch (err) {
    console.error(err);
    }
}

getData();

let currentRow = null;
let editRow = null;

function editProduct(button) {

    const row = button.closest("tr");

    if (currentRow === row) {
        cancelEdit();
        return;
    }

    currentRow = row;

    document.getElementById("productId").value = row.dataset.id;
    document.getElementById("name").value = row.querySelector(".name").innerText;
    document.getElementById("price").value = row.querySelector(".price").innerText;
    document.getElementById("category").value = row.querySelector(".category").innerText;
    document.getElementById("stock").value = row.querySelector(".stock").innerText;
    
    editRow = document.createElement("tr");
    const td = document.createElement("td");
    td.colSpan = 6;

    const form = document.getElementById("editForm");
    form.style.display = "block";

    td.appendChild(form);
    editRow.appendChild(td);

    row.parentNode.insertBefore(editRow, row.nextSibling);
}
function cancelEdit() {
    document.getElementById("editForm").style.display = "none";

    document.body.appendChild(document.getElementById("editForm"));

    if (editRow) editRow.remove();
    editRow = null;
    currentRow = null;
}

async function saveProduct() {
    if (!currentRow) return;

    const id = document.getElementById("productId").value;
    const name = document.getElementById("name").value;
    const price = document.getElementById("price").value;
    const category = document.getElementById("category").value;
    const stock = document.getElementById("stock").value;

    try {
        const res = await fetch(`/api/v1/products/${id}`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ name, price, category, stock })
        });

        if (!res.ok) throw new Error("Update failed");

        currentRow.querySelector(".name").innerText = name;
        currentRow.querySelector(".price").innerText = price;
        currentRow.querySelector(".category").innerText = category;
        currentRow.querySelector(".stock").innerText = stock;

        cancelEdit();

        alert("Produkt uppdaterad!");
    } catch (err) {
        console.error(err);
    }
}

function addProduct() {
    const thisProductForm = document.getElementById('addProductForm');
    thisProductForm.addEventListener('submit', async function (e) {
        e.preventDefault();
        const formData = new FormData(thisProductForm).entries()
        const response = await fetch('/api/v1/products', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(Object.fromEntries(formData))
        });

        location.reload();

        const result = await response.json();
        console.log(result)
    });
}

document.addEventListener("click", async (e) => {
    if (!e.target.classList.contains("delete-btn")) return;

    const productId = e.target.dataset.id;
    const row = e.target.closest("tr");

    if (!confirm("Delete this product?")) return;

    try {
        const res = await fetch(`/api/v1/products/${productId}`, {
            method: "DELETE"
        });

        if (!res.ok) throw new Error("Delete failed");

        // Remove only THIS row
        row.remove();

    } catch (err) {
        console.error(err);
        alert("Could not delete product");
    }
});

