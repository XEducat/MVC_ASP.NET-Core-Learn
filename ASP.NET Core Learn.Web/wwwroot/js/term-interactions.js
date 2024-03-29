var addButton = document.getElementById("add-term-button");
var removeButton = document.getElementById("remove-term-button");
var termListContainer = document.getElementById("term-list-container");
var inputTemplate = document.getElementById("input-template");

addButton.addEventListener("click", addInput);
removeButton.addEventListener("click", removeInput);

function addInput() {
    var newIndex = termListContainer.children.length; // Получаем текущее количество элементов
    var inputClone = inputTemplate.content.cloneNode(true);
    inputClone.querySelector('.term-input').name = `terms[${newIndex}].NumberMonths`; // Присваиваем правильный индекс
    termListContainer.appendChild(inputClone);
}

function removeInput() {
    var lastTermItem = termListContainer.lastElementChild;
    if (lastTermItem) {
        lastTermItem.remove();
    }
}

document.querySelector('form').addEventListener('submit', function (event) {
    var termInputs = document.querySelectorAll('.term-input');
    var terms = [];

    termInputs.forEach(function (input) {
        var termValue = input.value.trim();
        if (termValue !== "") {
            terms.push({ NumberMonths: termValue });
        }
    });
});