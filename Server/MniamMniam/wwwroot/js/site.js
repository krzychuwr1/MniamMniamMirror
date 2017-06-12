// Write your Javascript code.
$(function () {
    $('#anotherIngredient').on('click', function () {
        $('#ingredientsList').append("<div class='form-group'>" + $('#firstIngredient').html() + "</div>")
    });
});