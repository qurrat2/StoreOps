window.storeOpsCharts = (function () {
    const instances = {};

    function destroy(canvasId) {
        if (instances[canvasId]) {
            instances[canvasId].destroy();
            delete instances[canvasId];
        }
    }

    function renderTopProducts(canvasId, labels, quantities) {
        destroy(canvasId);
        const ctx = document.getElementById(canvasId);
        if (!ctx) return;

        instances[canvasId] = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: labels,
                datasets: [{
                    label: 'Units sold',
                    data: quantities,
                    backgroundColor: 'rgba(79, 70, 229, 0.85)',
                    hoverBackgroundColor: 'rgba(55, 48, 163, 1)',
                    borderRadius: 6,
                    barThickness: 24
                }]
            },
            options: {
                indexAxis: 'y',
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: { display: false },
                    tooltip: {
                        backgroundColor: '#0f172a',
                        padding: 10,
                        titleFont: { weight: '600' },
                        bodyFont: { size: 12 }
                    }
                },
                scales: {
                    x: {
                        beginAtZero: true,
                        grid: { color: '#f1f2f4' },
                        ticks: { color: '#64748b', font: { size: 11 } }
                    },
                    y: {
                        grid: { display: false },
                        ticks: { color: '#334155', font: { size: 12, weight: '500' } }
                    }
                }
            }
        });
    }

    function renderTrend(canvasId, labels, orderCounts, revenue) {
        destroy(canvasId);
        const ctx = document.getElementById(canvasId);
        if (!ctx) return;

        instances[canvasId] = new Chart(ctx, {
            type: 'line',
            data: {
                labels: labels,
                datasets: [
                    {
                        label: 'Orders',
                        data: orderCounts,
                        borderColor: '#4f46e5',
                        backgroundColor: 'rgba(79, 70, 229, 0.08)',
                        tension: 0.35,
                        fill: true,
                        yAxisID: 'yOrders',
                        pointBackgroundColor: '#4f46e5',
                        pointRadius: 3,
                        pointHoverRadius: 5,
                        borderWidth: 2
                    },
                    {
                        label: 'Revenue',
                        data: revenue,
                        borderColor: '#059669',
                        backgroundColor: 'rgba(5, 150, 105, 0.08)',
                        tension: 0.35,
                        fill: false,
                        yAxisID: 'yRevenue',
                        pointBackgroundColor: '#059669',
                        pointRadius: 3,
                        pointHoverRadius: 5,
                        borderWidth: 2,
                        borderDash: [5, 3]
                    }
                ]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                interaction: { mode: 'index', intersect: false },
                plugins: {
                    legend: {
                        position: 'top',
                        align: 'end',
                        labels: { usePointStyle: true, boxWidth: 8, color: '#334155', font: { size: 12 } }
                    },
                    tooltip: {
                        backgroundColor: '#0f172a',
                        padding: 10,
                        titleFont: { weight: '600' },
                        callbacks: {
                            label: function (context) {
                                const label = context.dataset.label || '';
                                const v = context.parsed.y;
                                return label === 'Revenue'
                                    ? `${label}: $${v.toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 })}`
                                    : `${label}: ${v}`;
                            }
                        }
                    }
                },
                scales: {
                    x: {
                        grid: { display: false },
                        ticks: { color: '#64748b', font: { size: 11 } }
                    },
                    yOrders: {
                        type: 'linear',
                        position: 'left',
                        beginAtZero: true,
                        grid: { color: '#f1f2f4' },
                        ticks: { color: '#4f46e5', font: { size: 11 }, stepSize: 1 },
                        title: { display: true, text: 'Orders', color: '#4f46e5', font: { size: 11 } }
                    },
                    yRevenue: {
                        type: 'linear',
                        position: 'right',
                        beginAtZero: true,
                        grid: { display: false },
                        ticks: {
                            color: '#059669',
                            font: { size: 11 },
                            callback: v => '$' + v.toLocaleString()
                        },
                        title: { display: true, text: 'Revenue', color: '#059669', font: { size: 11 } }
                    }
                }
            }
        });
    }

    return {
        renderTopProducts: renderTopProducts,
        renderTrend: renderTrend,
        destroy: destroy
    };
})();
