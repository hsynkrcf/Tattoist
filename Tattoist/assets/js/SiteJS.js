var data_table_language = {
    "decimal": "",
    "emptyTable": "Tabloda veri yok",
    "info": "Toplam <b> _TOTAL_ </b>sonucun<b> _START_ - _END_ </b>arası gösteriliyor.",
    "infoEmpty": "Sonuç yok",
    "infoFiltered": "( _MAX_ girdiden filtrelendi.)", //filtered from _MAX_ total entries
    "infoPostFix": "",
    "thousands": ",",
    "lengthMenu": "Bir sayfada  _MENU_  satır ",
    "loadingRecords": "Yükleniyor...",
    "processing": "Yükleniyor...",
    "search": "Ara:",
    "zeroRecords": "Benzer bir sonuç bulunamadı",
    "paginate": {
        "first": "İlk",
        "last": "Son",
        "next": "Sonraki",
        "previous": "Önceki"
    },
    "aria": {
        "sortAscending": ": a1ctivate to sort column ascending",
        "sortDescending": ": activate to sort column descending"
    }
};

var data_table_buttons = [
    {
        "extend": 'copy',
        "text": '<i class="far fa-copy"></i> Kopyala',
        "className": 'btn btn-brand'
    },
    {
        "extend": 'excel',
        "text": '<i class="fas fa-file-excel"></i> Excel',
        "className": 'btn btn-success'
    },
    //{
    //    "extend": 'csv',
    //    "text": '<i class="fas fa-file-csv"></i> CSV',
    //    "className": 'btn btn-danger',
    //    "exportOptions": {
    //        columns: [0, 1]
    //    }
    //},
    //{
    //    "extend": 'pdf',
    //    "text": '<i class="fas fa-file-pdf"></i> PDF',
    //    "className": 'btn btn-danger'
    //},
];

$(document).ready(function () {
    var table = $('#tabtab').DataTable({
        dom: 'lBfrtip',
        pageLength: 10,
        colReorder: true,
        language: data_table_language,
        buttons: data_table_buttons,
    });
});

    var turkish = {
        "format": "DD/MM/YYYY",
        "separator": " - ",
        "applyLabel": "Uygula",
        "cancelLabel": "Vazgeç",
        "fromLabel": "Dan",
        "toLabel": "a",
        "customRangeLabel": "Özel",
        "daysOfWeek": [
            "Pt",
            "Sl",
            "Çr",
            "Pr",
            "Cm",
            "Ct",
            "Pz"
        ],
        "monthNames": [
            "Ocak",
            "Şubat",
            "Mart",
            "Nisan",
            "Mayıs",
            "Haziran",
            "Temmuz",
            "Ağustos",
            "Eylül",
            "Ekim",
            "Kasım",
            "Aralık"
        ],
        "firstDay": 1
    };

    function OnlyNumber(evt) {
        var iKeyCode = evt.which ? evt.which : evt.keyCode;
        if ((iKeyCode >= 48 && iKeyCode <= 57) || iKeyCode === 46)
            return true;
        return false;
    }

$(function () {
    var start = moment();
    var end = moment();
        function cb(start, end) {
            $('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
        }

        $('#reportrange').daterangepicker({
            locale: turkish,
            ranges: {
                'Bugün': [moment(), moment()],
                'Dün': [moment().subtract(1,'days'), moment()],
                'Geçen Hafta': [moment().subtract(6, 'days'), moment()],
                'Geçen Ay': [moment().subtract(1, 'months'), moment()],
            }
        }, cb);

        cb(start, end);

    });