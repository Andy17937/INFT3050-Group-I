function isEmail(s) {
	if(s){
		return /^([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+((.[a-zA-Z0-9_-]{2,3}){1,2})$/.test(s)
	}
	return true;
}
function isMobile(s) {
	if(s){
		return /^1[0-9]{10}$/.test(s)
	}
	return true;
}
function isPhone(s) {
	if(s){
		return /^([0-9]{3,4}-)?[0-9]{7,8}$/.test(s)
	}
	return true;
}

function isURL(s) {
	if(s){
		return /^http[s]?:\/\/.*/.test(s)
	}
	return true;
}

function isNumber(s) {
	if(s){
		return /(^-?[+-]?([0-9]*\.?[0-9]+|[0-9]+\.?[0-9]*)([eE][+-]?[0-9]+)?$)|(^$)/.test(s);
	}
	return true;
}
function isIntNumer(s) {
	if(s){
		return /(^-?\d+$)|(^$)/.test(s);
	}
	return true;
}
function isIdentity(idcard) {
	const regIdCard = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/;
	if(idcard){
		return regIdCard.test(idcard);
	}
	return true;
}
