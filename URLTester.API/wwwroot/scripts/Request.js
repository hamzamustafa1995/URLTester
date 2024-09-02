$(document).ready(function () {

    // Function to fetch data and update the page
    function sendRequest() {
        var requestData = {
            Url: $('#url').val(),
            Method: $('#method').val(),
            Data: $('#data').val(),
            Token: $('#token').val()
        }

        $.ajax({
            url: '/URL/SendRequest', // Your API endpoint
            type: 'POST',
            contentType: 'application/json',
            dataType: 'json',
            data: JSON.stringify(requestData),
            success: function (data) {
                $('#nested-table').hide()
                $('#result').text(data.value); // Display result message
                $('#elapsedTime').text('Elapsed Time: ' + data.elapsedSeconds + ' s'); // Display elapsed time
                createTableFromJSON(data); // Create table with result         
            },
            error: function (xhr, status, error) {
                console.error('Error sending request:', error);
            }
        });
    }
    $('#sendRequestButton').click(function () {
        sendRequest();
    });
function createTableFromJSON(jsonData) {
    // Clear previous content
    $('#tableContainer').empty(); // Clear the table content
    $('.button-container').remove(); // Remove existing button containers
    let parsedValue = JSON.parse(jsonData.value); // Parse the value string
    jsonData.value = parsedValue;
    // Ensure the input is an array of objects for consistency
    let data = Array.isArray(jsonData) ? jsonData : [jsonData];

    // Determine which columns are objects
    let objectColumns = new Set();
    if (data.length > 0) {
        // Check the first row to determine which columns have object values
        for (let key in data[0]) {
            if (typeof data[0][key] === 'object' && data[0][key] !== null) {
                objectColumns.add(key);
            }
        }
    }

    // Create the main table
    let table = $('<table>').addClass('table table-bordered');
    let thead = $('<thead class="thead-dark">');
    let headerRow = $('<tr>');

    // Create table headers and add class to those corresponding to object columns
    for (let key in data[0]) {
        let headerCell = $('<th>').text(key);
        if (objectColumns.has(key)) {
            headerCell.addClass('nested-table');
        }
        headerRow.append(headerCell);
    }

    thead.append(headerRow);
    table.append(thead);

    // Create table rows
    let tbody = $('<tbody>');
    data.forEach((item, index) => {
        let row = $('<tr>');
        for (let key in item) {
            let cellValue = item[key];
            if (cellValue === null) {
                // If the value is null, create a cell with a placeholder text
                row.append($('<td>').text('No Data'));
            } else if (typeof cellValue === 'object' && cellValue !== null) {
                // If the value is an object, create a nested table and hide it initially
                let nestedTable = createNestedTable(cellValue);
                let cell = $('<td>')
                    .append(nestedTable)
                    .addClass('nested-table')
                    .hide(); // Initially hide the nested table
                row.append(cell);
            } else {
                // Otherwise, add a simple text cell
                row.append($('<td>').text(cellValue));
            }
        }
        tbody.append(row);
    });
    table.append(tbody);

    // Append the table to the container
    $('#tableContainer').html(table);

    // Handle the toggle button click event
    let addbreakPoints = '<br/><br/>';
    let buttonContainer = $('<div>').addClass('button-container');
    let toggleButton = $('<button>')
        .attr('id', 'toggle-all-btn')
        .text('Toggle All Visibility')
        .addClass('toggle-btn');
    buttonContainer.append(addbreakPoints);
    buttonContainer.append(toggleButton);
    $('#tableContainer').before(buttonContainer);

    // Handle the toggle button click event
    $('#toggle-all-btn').off('click').on('click', function () {
        $('.nested-table').each(function () {
            $(this).toggle(); // Toggle visibility of each nested table individually      
        });
    });

    $('.nested-table').hide(); 

}

 // Function to create a nested table from an object
 function createNestedTable(data) {
        // Create the main table
        let nestedTable = $('<table>').addClass('table table-bordered');
        let nestedThead = $('<thead>');
        let nestedHeaderRow = $('<tr>');

        // Determine if the data is an array or a single object
        let isArray = Array.isArray(data);

        // Create headers for the nested table using keys of the first object
        let firstObj = isArray && data.length > 0 ? data[0] : data;

        if (firstObj && typeof firstObj === 'object') {
            for (let key in firstObj) {
                nestedHeaderRow.append($('<th>').text(key));
            }
        }
        nestedThead.append(nestedHeaderRow);
        nestedTable.append(nestedThead);

        // Create rows for the nested table
        let nestedTbody = $('<tbody>');

        // Function to create rows for objects
        function createRows(obj) {
            let nestedRow = $('<tr>');
            for (let key in obj) {
                if (obj.hasOwnProperty(key)) {
                    if (typeof obj[key] === 'object' && obj[key] !== null) {
                        // Recursively create a nested table for nested objects
                        nestedRow.append($('<td>').append(createNestedTable(obj[key])));
                    } else {
                        // Add regular data cells
                        nestedRow.append($('<td>').text(obj[key]));
                    }
                }
            }
            return nestedRow;
        }

        if (isArray) {
            data.forEach(obj => {
                if (typeof obj === 'object') {
                    nestedTbody.append(createRows(obj));
                }
            });
        } else {
            if (typeof data === 'object') {
                nestedTbody.append(createRows(data));
            }
        }

        nestedTable.append(nestedTbody);

        return nestedTable;
    }
});