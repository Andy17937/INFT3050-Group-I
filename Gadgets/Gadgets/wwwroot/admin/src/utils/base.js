const base = {
    get() {
                return {
            url : "http://localhost:5000/",
            name: ".",
            // 退出到Home链接
            indexUrl: 'http://localhost:5000/front/index.html'
        };
            },
    getProjectName(){
        return {
            projectName: "Gadgets"
        } 
    }
}
export default base
