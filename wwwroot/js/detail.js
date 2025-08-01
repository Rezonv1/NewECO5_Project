$(document).ready(function () {
    console.log("✅ detail.js 已載入");

    const $searchField = $('#searchField');
    const $searchQuery = $('#searchQuery');
    const $tableBody = $('#meterListTable tbody');
    const $btnToggleList = $('#toggleMeterList');
    const offcanvasEl = document.getElementById('meterListCanvas');

    let meterListCanvas = null;
    if (offcanvasEl) {
        meterListCanvas = new bootstrap.Offcanvas(offcanvasEl, { backdrop: false });

        offcanvasEl.addEventListener('show.bs.offcanvas', function () {
            console.log("📂 Offcanvas 開啟");
            document.body.classList.add('offcanvas-opened');
        });

        offcanvasEl.addEventListener('hidden.bs.offcanvas', function () {
            console.log("📂 Offcanvas 關閉");
            document.body.classList.remove('offcanvas-opened');
        });
    }

    $btnToggleList.on('click', function () {
        console.log("🔵 點擊顯示電表清單按鈕");
        if (meterListCanvas) {
            meterListCanvas.show();
        } else {
            console.warn("⚠️ meterListCanvas 尚未初始化");
        }
    });

    function renderRows(data) {
        $tableBody.empty();
        if (data.length === 0) {
            $tableBody.append('<tr><td colspan="3" class="text-center">無符合條件的電表</td></tr>');
        } else {
            data.forEach(item => {
                const row = `
                    <tr data-id="${item.serialNr}" style="cursor: pointer;">
                        <td>${item.serialNr}</td>
                        <td>${item.deviceName || ''}</td>
                        <td>${item.description || ''}</td>
                    </tr>`;
                $tableBody.append(row);
            });
        }
    }

    function searchMeters() {
        $.get('/MetersGroupSetting/SearchMeters', {
            searchQuery: $searchQuery.val(),
            searchField: $searchField.val()
        }).done(renderRows).fail(() => alert('查詢失敗，請稍後再試'));
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

    $tableBody.on('click', 'tr', function (e) {
        if ($(e.target).closest('.btn').length === 0) {
            const id = $(this).data('id');
            if (id) window.location.href = `/MetersGroupSetting/Edit/${id}`;
        }
    });
});
