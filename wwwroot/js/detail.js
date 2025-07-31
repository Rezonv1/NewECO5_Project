// detail.js - 通訊設定頁面專用腳本

$(document).ready(function () {
    const $searchField = $('#searchField');
    const $searchQuery = $('#searchQuery');
    const $tableBody = $('#meterListTable tbody');

    function renderRows(data) {
        $tableBody.empty();
        if (data.length === 0) {
            $tableBody.append('<tr><td colspan="5" class="text-center">無符合條件的通訊設定</td></tr>');
        } else {
            data.forEach(item => {
                const row = `
                    <tr data-id="${item.id}">
                        <td>${item.description || '無'}</td>
                        <td>${item.isTCPSetting === 0 ? 'RS-485' : 'TCP'}</td>
                        <td>${item.ip || '無'}</td>
                        <td>${item.isTCPSetting === 1 ? (item.port || '無') : 'N/A'}</td>
                        <td>
                            <a href="/MetersGroupSetting/Edit/${item.id}" class="btn btn-sm btn-warning">編輯</a>
                            <a href="/MetersGroupSetting/Delete/${item.id}" class="btn btn-sm btn-danger">刪除</a>
                        </td>
                    </tr>`;
                $tableBody.append(row);
            });

            bindRowClickEvent();
        }
    }

    function bindRowClickEvent() {
        $('.table-clickable tbody tr').off('click').on('click', function (e) {
            if ($(e.target).hasClass('btn') || $(e.target).closest('.btn').length) return;
            const id = $(this).data('id');
            if (id) window.location.href = `/MetersGroupSetting/Edit/${id}`;
        });
    }

    function searchMeters() {
        const query = $searchQuery.val();
        const field = $searchField.val();

        $.ajax({
            url: '/MetersGroupSetting/SearchMeters',
            type: 'GET',
            data: { searchQuery: query, searchField: field },
            success: renderRows,
            error: function () {
                alert('查詢失敗，請稍後重試。');
            }
        });
    }

    function debounce(func, wait) {
        let timeout;
        return function () {
            clearTimeout(timeout);
            timeout = setTimeout(func, wait);
        };
    }

    $searchQuery.on('input', debounce(searchMeters, 300));
    $searchField.on('change', searchMeters);
});
