
layui.define(['jquery', 'layer'], function(exports) {
	"use strict";
	var jquery = layui.jquery,
		layer = layui.layer,
		        baseurl = "http://localhost:5000/";
        	var http = {
		        domain : "http://localhost:5000/",
        		baseurl: baseurl,
		getParam: function(name) {
			var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
			var r = window.location.search.substr(1).match(reg);
			if (r != null)
				return decodeURI(r[2]);
			return null;
		},
		request: function(url, type, data, callback) {
			var index = layer.load(1, {
				shade: [0.1, '#fff']
			});
			url = baseurl + url;
			data['t'] = jquery.now();
			jquery.ajax({
				url: url,
				beforeSend: function(request) {
					request.setRequestHeader("Token", localStorage.getItem("Token"));
				},
				contentType: 'application/x-www-form-urlencoded',
				data: data,
				dataType: 'json',
				type: type,
				success: function(result, status, xhr) {
					if (result.code == 0) {
						callback(result);
					} else if (result.code == 401 || result.code == 403) {
						window.parent.location.href = '../login/login.html';
					} else {
						layer.msg(result.msg, {
							time: 2000,
							icon: 5
						})
					}
					layer.close(index);
				},
				error: function(xhr, status, error) {
					console.log(xhr, status, error)
					layer.msg("Request Interface Failure", {
						time: 2000,
						icon: 5
					})
					layer.close(index);
				}
			});
		},
		requestJson: function(url, type, data, callback) {
			var index = layer.load(1, {
				shade: [0.1, '#fff']
			});
			url = baseurl + url;
			var params = null;
			data['t'] = jquery.now();
			if (data) {
				params = JSON.stringify(data);
			}
			jquery.ajax({
				url: url,
				beforeSend: function(request) {
					request.setRequestHeader("Token", localStorage.getItem("Token"));
				},
				contentType: 'application/json',
				data: params,
				dataType: 'json',
				type: type,
				success: function(result, status, xhr) {
					if (result.code == 0) {
						callback(result);
					} else if (result.code == 401 || result.code == 403) {
						window.parent.location.href = '../login/login.html';
					} else {
						layer.msg(result.msg, {
							time: 2000,
							icon: 5
						})
					}
					layer.close(index);
				},
				error: function(xhr, status, error) {
					console.log(xhr, status, error)
					layer.msg("Request Interface Failure", {
						time: 2000,
						icon: 5
					})
					layer.close(index);
				},
			});
		},
		upload: function(file, fileName, callback) {
			var url = baseurl + "file/upload";
			var formData = new FormData();
			formData.append('file', file);
			formData.append('fileName', fileName);
			jquery.ajax({
				url: url,
				type: 'post',
				data: formData,
				headers: {
					"Token": localStorage.getItem("Token")
				},
				contentType: false,
				processData: false,
				success: function(res) {
					if (res.code == 0) {
						callback(res);
					} else if (res.code == 401 || result.code == 403) {
						window.parent.location.href = '../login/login.html';
					} else {
						layer.msg(res.msg, {
							time: 2000,
							icon: 5
						})
					}
				}
			})
		}
	}
	exports('http', http);
});
