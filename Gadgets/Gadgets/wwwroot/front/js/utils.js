function jump(url) {
	if (!url || url == 'null' || url == null) {
		window.location.href = './index.html';
	}
	localStorage.setItem('iframeUrl', url.replace('../', './pages/'));
	window.location.href = url;
}

function back(num = -1) {
	window.history.go(num)
}

function genTradeNo() {
	var date = new Date();
	var tradeNo = date.getFullYear().toString() + (date.getMonth() + 1).toString() +
		date.getDate().toString() + date.getHours().toString() + date.getMinutes().toString() +
		date.getSeconds().toString() + date.getMilliseconds().toString();
	for (var i = 0; i < 5; i++)
	{
		tradeNo += Math.floor(Math.random() * 10);
	}
	return tradeNo;
}
