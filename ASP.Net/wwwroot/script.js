//async function doAjax(url) {
//    let result = await $.getJSON(url);// функция jquery, AJAX запрос происходит к серверу Kestrel на получение файла
//    return result;//из-за await ожидния данные обертываются в обьект promise
//}
//$(function ()//обработчик готовности DOM повесили с помощью  $-jquery, можем обращаться к любому элементу штмл страницы зная что они уже есть.
//{
//    doAjax(//возвращает обьект promise который обещает вернуть данные позже, на место своего вызова
//        "cities.json"
//    ).then((data) =>//на обьекте что вернула функция doAjax (запрос выполнится, промис перейдёт в состояние успех) вызываем метод then , регестрирует подписчиков хорошо 200 и плохо 404 и в data попадёт ответ (JSON-данные). Когда будут готовы данные подписчик активизируется.
//    {
//        for (let i = 0; i < data.cities.length; i++) {
//            $('#cities').append('<tr><td>' + data.cities[i].name +
//                '</td><td>' + data.cities[i].population + '</td><tr>');
//        }
//    });

//    //тут уже без promise
//    $.getJSON('countries.json', function (data) {
//        $.each(data, function (key, val)//на подобии foreach
//        {
//            $('#countries').append('<option value="' + val + '">' + key + '</option>');
//        });


//        $('#countries').on('change', function (e) {//обработчик события change на комбобокс
//            $("#city").text($("#countries option:selected").val());
//        });
//    });
//});


async function doAjax(url) {
    try
    {
        let result = await $.getJSON(url);
        return result;
    }
    catch (error)
    {
        $('#errorState').removeClass('hidden');
    }
}

$(function () {
    $('#errorState').addClass('hidden');
    function searchMovie() {
        const movieTitle = $('#searchInput').val();
        let apiKey = "547df6c8";
        let apiUrl = `https://www.omdbapi.com/?apikey=${apiKey}&t=${encodeURIComponent(movieTitle)}`;


        doAjax(apiUrl).then((data) => {
            console.log("Released: ", data.Released);
            if (data && data.Response !== "False")
            {
                $('#errorState').addClass('hidden'); 

                $('#movieTitle').append(data.Title);
                $('#rating').append(data.imdbRating + '/10');
                $('#year').append(data.Year);
                $('#released').append(data.Released);
                $('#runtime').append(data.Runtime);
                $('#genre').append(data.Genre);
                $('#director').append(data.Director);
                $('#writer').append(data.Writer);
                $('#actors').append(data.Actors);
                $('#plot').append(data.Plot);
                $('#language').append(data.Language);
                $('#country').append(data.Country);
                $('#awards').append(data.Awards);


                const posterUrl = data.Poster;
                $('#moviePoster').attr({
                    src: posterUrl,
                    alt: data.Title
                });

                $('#movieDetails').removeClass('hidden');
            }
            else {
                $('#loadingState').addClass('hidden');
                $('#errorState').removeClass('hidden'); 
            }
        });
    }

    function clearMovie() {
        $('#searchInput').val('');
        $('#rating').text('');
        $('#moviePoster').attr('src', ''); 
        $('#movieTitle').text('');
        $('#year').text('');
        $('#released').text('');
        $('#runtime').text('');
        $('#genre').text('');
        $('#director').text('');
        $('#writer').text('');
        $('#actors').text('');
        $('#plot').text('');
        $('#language').text('');
        $('#country').text('');
        $('#awards').text('');

        $('#movieDetails').addClass('hidden');
    }

   
    $('#searchBtn').click(searchMovie);
    $('#backBtn').click(clearMovie);


  
});
