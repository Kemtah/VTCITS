$.extend($.jgrid.defaults, {
    datatype: 'json',
    mtype: 'GET',
    rowNum: 10,
    rowList: [10, 20, 30],
    autoWidth: true,
    forceFit: true,
    height: '100%',
    hidegrid: false,
    jsonReader: {
        cell: '',
        total: '',
        page: ''
    },
    loadonce: true,
    loadui: 'block',
    sortorder: 'desc',
    viewrecords: true,
    beforeSelectRow: function (rowid, e) {
        return false;
    }
});

