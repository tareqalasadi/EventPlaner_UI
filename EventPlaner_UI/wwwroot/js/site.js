
var dataContainer = document.getElementById('card-container-service');
var allItems = JSON.parse(dataContainer.getAttribute('data-items'));
$(document).ready(function () {
    debugger;
    

    initialize(allItems);  
});

function initialize(items) {
    renderCards(items);  
    document.getElementById('typeService').addEventListener('change', filterCards);
}

function renderCards(items) {
    const cardContainer = document.getElementById('card-container-service');
    cardContainer.innerHTML = '';

    items.forEach(item => {
        const cardHtml = `
            <div class="col-6 align-items-stretch" data-aos="fade-up" data-aos-delay="100">
                <div class="chef-member" 
                     data-image="data:image/png;base64,${item.ImageBase64}" 
                     data-price="${item.Price}" 
                     data-serviceid="${item.ID}"> <!-- Add ServiceID to the div -->
                    <div class="member-img">
                        <img src="data:image/png;base64,${item.ImageBase64}" class="img-fluid service-image" alt="Service Image">
                    </div>
                    <div class="member-info">
                        <p>Price: ${item.Price}</p>
                    </div>
                    <div class="member-infoo" hidden>
                        <p>${item.ImageNameEn}</p>
                    </div>
                    <div class="card-footer">
                    <div style="    padding-top: 10px;
">
                     <div class="custom-checkbox custom-control">
                            <input type="checkbox" class="custom-control-input select-item" id="select-item-${item.ID}" value="${item.ID}">
                            <label class="custom-control-label" for="select-item-${item.ID}">Select</label>
                     </div>
                    </div>
                       
                    </div>
                </div>
            </div>`;
        cardContainer.innerHTML += cardHtml;
    });

    // Attach event listeners to all checkboxes
    document.querySelectorAll('.select-item').forEach(checkbox => {
        checkbox.addEventListener('change', function () {
           
            handleCardSelection(this);
        });
    });
}



function handleCardSelection(checkbox) {
    const cardElement = checkbox.closest('.chef-member');
    const selectedServiceID = cardElement.getAttribute('data-serviceid');

    if (checkbox.checked) {
        var submitButton = document.getElementById('submit-button');
        if (submitButton) {
            submitButton.disabled = false;
        }
        const selectedImage = cardElement.getAttribute('data-image');
        const selectedPrice = cardElement.getAttribute('data-price');
        const selectedName = cardElement.querySelector('.member-infoo p').innerText;

        if (!cardElement.querySelector('.quantity-selector')) {
            const quantitySelectorHtml = `
            <div class="quantity-selector d-flex align-items-center justify-content-between mt-2" style="
    margin-left: 13px;
    margin-bottom: 5px;
">
                <button class="btn btn-sm btn-decrement" data-serviceid="${selectedServiceID}">
                    <i class="fa fa-minus"></i>
                </button>
                <input type="text" class="form-control form-control-sm quantity-input text-center mx-1" data-serviceid="${selectedServiceID}" value="1" readonly style="width: 50px;">
                <button class="btn btn-sm btn-increment" data-serviceid="${selectedServiceID}">
                    <i class="fa fa-plus"></i>
                </button>
            </div>`;

            const cardFooter = cardElement.querySelector('.card-footer');
            cardFooter.insertAdjacentHTML('beforeend', quantitySelectorHtml);

            cardElement.querySelector('.btn-increment').addEventListener('click', function () {
                updateQuantity(this, 1);
            });
            cardElement.querySelector('.btn-decrement').addEventListener('click', function () {
                updateQuantity(this, -1);
            });
        }

    } else {
        const checkboxes = document.querySelectorAll('.chef-member input[type="checkbox"]');
        const isAnyChecked = Array.from(checkboxes).some(cb => cb.checked);

        var submitButton = document.getElementById('submit-button');
        if (submitButton) {
            submitButton.disabled = !isAnyChecked; 
        }
        const quantitySelector = cardElement.querySelector('.quantity-selector');
        if (quantitySelector) quantitySelector.remove();
    }
}


function updateQuantity(button, change) {
    const cardElement = button.closest('.chef-member');
    const serviceID = button.getAttribute('data-serviceid');
    const quantityInput = cardElement.querySelector(`.quantity-input[data-serviceid='${serviceID}']`);
    let currentQuantity = parseInt(quantityInput.value);

    currentQuantity += change;
    if (currentQuantity < 1) currentQuantity = 1; 
    quantityInput.value = currentQuantity;

    
}


function filterCards() {
    debugger;
    const selectedType = document.getElementById('typeService2').value;
    const filteredItems = selectedType ? allItems.filter(item => item.TypeService === selectedType) : allItems;
    renderCards(filteredItems);
}
