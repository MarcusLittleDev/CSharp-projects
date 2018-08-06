$(document).ready(function () {
    $("#role_add").click(function (event) {
        event.preventDefault();
        var data = (this).getAttribute("data-role");
        
        $.ajax({
            type: "GET",
            url: "/Actors/GetModel",
            data: { id: data},
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            success: function (response) {
                if (response != null) {

                    var markup ='<tr id="submit_row"><td><select class="movie" name="MovieID" required><option value=""></option>';

                
                    for (var i = 0; i < response.length; i++) {
                        var obj = response[i];
                        markup += '<option value="' + obj.id + '">' + obj.title + '</option>'
                    }
                    markup += '</select></td><td><input type="text" id="character" name="Character" value="" required/></td><td nowrap="nowrap"><input type="submit" class="save_row btn btn-primary" value="Save"></button><button class="remove_row btn">Cancel</button></td></tr>'
               
                    $("#role_table").append(markup);
                }
                else {
                    alert("something whent wrong")
                }  
            
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        } 
    })
    });
    $(document).on('click', '.remove_row', function () {
        $(this).closest('tr').remove();
    });

    $("#role_form").submit(function (e) {
        e.preventDefault();
        var jsonObject = $('#role_form').serializeArray();
        $.ajax({
            url: '/Actors/SaveRow',
            //contentType: 'application/json; charset=utf-8',
            type: 'POST',
            data: jsonObject ,
            dataType: "json",
            error: function (response) {
                alert(response.responseText);
            },
            success: function (response) {
                var markup = "<tr><td>" + response.Title + "</td><td>" + response.Character + "</td><td></td></tr>"
                var row = document.getElementById('submit_row');
                var table = document.getElementById('role_table');
                table.deleteRow(table.rows.length - 1);
                $('#role_table').append(markup);
                
                
            }
        })
    });

})