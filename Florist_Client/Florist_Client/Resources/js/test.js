$('.header-search-input').on('input', function (e) {
    document.querySelector('.header-search-icon').href = '/SearchProduct?searchName=' + $('.header-search-input').val();
})