let alatList = [];
let materijalList = [];

$(document).ready(function () {
    const pageUrl = window.location.pathname;

    if (pageUrl.endsWith('alati.html')) {
        loadAlati();
        $('#alatForm').on('submit', saveAlat);
        $('#cancelAlatEdit').on('click', function () {
            $('#alatForm')[0].reset();
            $('#alatId').val('');
        });
        $(document).on('click', '.edit-alat', function () {
            const id = $(this).data('id');
            editAlat(id);
        });
        $(document).on('click', '.delete-alat', function () {
            const id = $(this).data('id');
            deleteAlat(id);
        });
    } else if (pageUrl.endsWith('materijali.html')) {
        loadMaterijali();
        $('#materijalForm').on('submit', saveMaterijal);
        $('#cancelMaterijalEdit').on('click', function () {
            $('#materijalForm')[0].reset();
            $('#materijalId').val('');
        });
        $(document).on('click', '.edit-materijal', function () {
            const id = $(this).data('id');
            editMaterijal(id);
        });
        $(document).on('click', '.delete-materijal', function () {
            const id = $(this).data('id');
            deleteMaterijal(id);
        });
    } else if (pageUrl.endsWith('narudzbe.html')) {
        loadNarudzbe();
        $(document).on('click', '.delete-narudzba', function () {
            const id = $(this).data('id');
            deleteNarudzba(id);
        });
    } else {
        // index.html
        // Fetch products
        $.getJSON('/api/alati', function (alati) {
            alatList = alati;
        }).fail(function () {
            alert('Greška prilikom dohvaćanja alata.');
        });

        $.getJSON('/api/materijali', function (materijali) {
            materijalList = materijali;
        }).fail(function () {
            alert('Greška prilikom dohvaćanja materijala.');
        });

        $('#narudzbaForm').on('submit', saveNarudzba);
        $('#dodajStavku').on('click', dodajStavku);
    }
});

/* Alati Functions */
function loadAlati() {
    $.getJSON('/api/alati', function (alati) {
        const alatiTable = $('#alatiTable');
        alatiTable.empty();
        $.each(alati, function (index, alat) {
            const row = $('<tr></tr>');
            row.append(`<td>${alat.naziv}</td>`);
            row.append(`<td>${alat.sku}</td>`);
            row.append(`<td>${alat.kolicina}</td>`);
            row.append(`<td>${alat.cijena.toFixed(2)} kn</td>`);
            row.append(`<td>${alat.marka || ''}</td>`);
            row.append(`<td>${alat.garancijaGodina || ''}</td>`);
            row.append(`<td>
                <button class="edit-alat" data-id="${alat.id}">Uredi</button>
                <button class="delete-alat" data-id="${alat.id}">Obriši</button>
            </td>`);
            alatiTable.append(row);
        });
    });
}

function saveAlat(event) {
    event.preventDefault();
    const alatId = $('#alatId').val();

    // Create the alat object
    const alat = {
        naziv: $('#naziv').val(),
        sku: $('#sku').val(),
        kolicina: parseInt($('#kolicina').val()),
        cijena: parseFloat($('#cijena').val()),
        marka: $('#marka').val() || null,
        garancijaGodina: $('#garancijaGodina').val() ? parseInt($('#garancijaGodina').val()) : null
    };

    if (alatId) {
        // Update existing alat
        alat.id = parseInt(alatId); // 'id' is added to 'alat' here
    }

    // Prepare AJAX options
    let ajaxOptions = {
        url: '/api/alati',
        contentType: 'application/json',
        data: JSON.stringify(alat), // 'data' is set after 'id' is added
        success: function () {
            alert('Alat je uspješno spremljen.');
            $('#alatForm')[0].reset();
            $('#alatId').val('');
            loadAlati();
        },
        error: function (jqxhr) {
            alert('Greška prilikom spremanja alata: ' + jqxhr.responseText);
        }
    };

    if (alatId) {
        // Update existing alat
        ajaxOptions.type = 'PUT';
        ajaxOptions.url += `/${alatId}`;
    } else {
        // Create new alat
        ajaxOptions.type = 'POST';
    }

    $.ajax(ajaxOptions);
}


function editAlat(id) {
    $.getJSON(`/api/alati/${id}`, function (alat) {
        $('#alatId').val(alat.id);
        $('#naziv').val(alat.naziv);
        $('#sku').val(alat.sku);
        $('#kolicina').val(alat.kolicina);
        $('#cijena').val(alat.cijena);
        $('#marka').val(alat.marka);
        $('#garancijaGodina').val(alat.garancijaGodina);
    });
}

function deleteAlat(id) {
    if (confirm('Jeste li sigurni da želite obrisati ovaj alat?')) {
        $.ajax({
            url: `/api/alati/${id}`,
            type: 'DELETE',
            success: function () {
                alert('Alat je uspješno obrisan.');
                loadAlati();
            },
            error: function (jqxhr) {
                alert('Greška prilikom brisanja alata: ' + jqxhr.responseText);
            }
        });
    }
}

/* Materijali Functions */
function loadMaterijali() {
    $.getJSON('/api/materijali', function (materijali) {
        const materijaliTable = $('#materijaliTable');
        materijaliTable.empty();
        $.each(materijali, function (index, materijal) {
            const row = $('<tr></tr>');
            row.append(`<td>${materijal.naziv}</td>`);
            row.append(`<td>${materijal.sku}</td>`);
            row.append(`<td>${materijal.kolicina}</td>`);
            row.append(`<td>${materijal.cijena.toFixed(2)} kn</td>`);
            row.append(`<td>${materijal.tip || ''}</td>`);
            row.append(`<td>${materijal.dobavljac || ''}</td>`);
            row.append(`<td>
                <button class="edit-materijal" data-id="${materijal.id}">Uredi</button>
                <button class="delete-materijal" data-id="${materijal.id}">Obriši</button>
            </td>`);
            materijaliTable.append(row);
        });
    });
}

function saveMaterijal(event) {
    event.preventDefault();
    const materijalId = $('#materijalId').val();

    // Create the materijal object
    const materijal = {
        naziv: $('#naziv').val(),
        sku: $('#sku').val(),
        kolicina: parseInt($('#kolicina').val()),
        cijena: parseFloat($('#cijena').val()),
        tip: $('#tip').val() || null,
        dobavljac: $('#dobavljac').val() || null
    };

    if (materijalId) {
        // Update existing materijal
        materijal.id = parseInt(materijalId); // Add 'id' before serialization
    }

    // Prepare AJAX options
    let ajaxOptions = {
        url: '/api/materijali',
        contentType: 'application/json',
        data: JSON.stringify(materijal), // Serialize after adding 'id'
        success: function () {
            alert('Materijal je uspješno spremljen.');
            $('#materijalForm')[0].reset();
            $('#materijalId').val('');
            loadMaterijali();
        },
        error: function (jqxhr) {
            alert('Greška prilikom spremanja materijala: ' + jqxhr.responseText);
        }
    };

    if (materijalId) {
        // Update existing materijal
        ajaxOptions.type = 'PUT';
        ajaxOptions.url += `/${materijalId}`;
    } else {
        // Create new materijal
        ajaxOptions.type = 'POST';
    }

    $.ajax(ajaxOptions);
}

function editMaterijal(id) {
    $.getJSON(`/api/materijali/${id}`, function (materijal) {
        $('#materijalId').val(materijal.id);
        $('#naziv').val(materijal.naziv);
        $('#sku').val(materijal.sku);
        $('#kolicina').val(materijal.kolicina);
        $('#cijena').val(materijal.cijena);
        $('#tip').val(materijal.tip);
        $('#dobavljac').val(materijal.dobavljac);
    });
}

function deleteMaterijal(id) {
    if (confirm('Jeste li sigurni da želite obrisati ovaj materijal?')) {
        $.ajax({
            url: `/api/materijali/${id}`,
            type: 'DELETE',
            success: function () {
                alert('Materijal je uspješno obrisan.');
                loadMaterijali();
            },
            error: function (jqxhr) {
                alert('Greška prilikom brisanja materijala: ' + jqxhr.responseText);
            }
        });
    }
}

/* Narudzbe Functions */
function loadNarudzbe() {
    $.getJSON('/api/narudzbe', function (narudzbe) {
        const narudzbeTable = $('#narudzbeTable');
        narudzbeTable.empty();
        $.each(narudzbe, function (index, narudzba) {
            const row = $('<tr></tr>');
            row.append(`<td>${narudzba.id}</td>`);
            row.append(`<td>${new Date(narudzba.datum).toLocaleDateString('hr-HR')}</td>`);
            row.append(`<td>${narudzba.ukupnaCijena.toFixed(2)} kn</td>`);
            row.append(`<td>${narudzba.napomena || ''}</td>`);
            row.append(`<td>
                <button class="delete-narudzba" data-id="${narudzba.id}">Obriši</button>
            </td>`);
            narudzbeTable.append(row);
        });
    });
}

function dodajStavku() {
    const stavkeContainer = $('#stavkeContainer');
    const stavkaHtml = `
        <div class="stavka">
            <label>Proizvod Tip:</label>
            <select class="proizvodTip" required>
                <option value="">Odaberite tip</option>
                <option value="Alat">Alat</option>
                <option value="Materijal">Materijal</option>
            </select>
            <label>Proizvod:</label>
            <select class="proizvodSelect" required>
                <option value="">Odaberite proizvod</option>
            </select>
            <label>Količina:</label>
            <input type="number" class="kolicina" required>
            <label>Jedinicna Cijena:</label>
            <input type="number" class="jedinicnaCijena" step="0.01" required>
            <button type="button" class="obrisiStavku">Obriši</button>
        </div>
    `;
    stavkeContainer.append(stavkaHtml);
}

$(document).on('change', '.proizvodTip', function () {
    const proizvodTip = $(this).val();
    const proizvodSelect = $(this).closest('.stavka').find('.proizvodSelect');

    let options = '<option value="">Odaberite proizvod</option>';
    if (proizvodTip === 'Alat') {
        options += alatList.map(alat => `<option value="${alat.id}">${alat.naziv}</option>`).join('');
    } else if (proizvodTip === 'Materijal') {
        options += materijalList.map(materijal => `<option value="${materijal.id}">${materijal.naziv}</option>`).join('');
    }
    proizvodSelect.html(options);
});

$(document).on('click', '.obrisiStavku', function () {
    $(this).closest('.stavka').remove();
});

function saveNarudzba(event) {
    event.preventDefault();

    const stavke = [];
    let valid = true;

    $('#stavkeContainer .stavka').each(function () {
        const proizvodId = parseInt($(this).find('.proizvodSelect').val());
        const proizvodTip = $(this).find('.proizvodTip').val();
        const kolicina = parseInt($(this).find('.kolicina').val());
        const jedinicnaCijena = parseFloat($(this).find('.jedinicnaCijena').val());

        if (!proizvodId || !proizvodTip || !kolicina || !jedinicnaCijena) {
            alert('Molimo popunite sve podatke za stavku.');
            valid = false;
            return false; // Break out of the loop
        }

        const stavka = {
            proizvodId: proizvodId,
            proizvodTip: proizvodTip,
            kolicina: kolicina,
            jedinicnaCijena: jedinicnaCijena
        };
        stavke.push(stavka);
    });

    if (!valid) return;

    const narudzba = {
        napomena: $('#napomena').val() || null,
        stavke: stavke
    };

    // Optionally calculate total price on the client side
    narudzba.ukupnaCijena = stavke.reduce((total, stavka) => total + (stavka.kolicina * stavka.jedinicnaCijena), 0);

    $.ajax({
        url: '/api/narudzbe',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(narudzba),
        success: function () {
            alert('Narudžba je uspješno kreirana.');
            $('#narudzbaForm')[0].reset();
            $('#stavkeContainer').empty();
        },
        error: function (jqxhr) {
            alert('Greška prilikom kreiranja narudžbe: ' + jqxhr.responseText);
        }
    });
}

function deleteNarudzba(id) {
    if (confirm('Jeste li sigurni da želite obrisati ovu narudžbu?')) {
        $.ajax({
            url: `/api/narudzbe/${id}`,
            type: 'DELETE',
            success: function () {
                alert('Narudžba je uspješno obrisana.');
                loadNarudzbe();
            },
            error: function (jqxhr) {
                alert('Greška prilikom brisanja narudžbe: ' + jqxhr.responseText);
            }
        });
    }
}
