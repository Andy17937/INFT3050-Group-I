<template>
  <div>
    <el-form
      class="detail-form-content"
      ref="ruleForm"
      :model="ruleForm"
      label-width="80px"
    >  
     <el-row>
                              <el-col :span="12">
        <el-form-item   v-if="flag=='huiyuan'"  label="Member Name" prop="huiyuanming">
          <el-input v-model="ruleForm.huiyuanming" readonly              placeholder="Member Name" clearable></el-input>
        </el-form-item>
      </el-col>
                                          <el-col :span="12">
        <el-form-item   v-if="flag=='huiyuan'"  label="Phone" prop="shouji">
          <el-input v-model="ruleForm.shouji"               placeholder="Phone" clearable></el-input>
        </el-form-item>
      </el-col>
                        <el-col :span="12">
        <el-form-item   v-if="flag=='huiyuan'"  label="Email" prop="youxiang">
          <el-input v-model="ruleForm.youxiang"               placeholder="Email" clearable></el-input>
        </el-form-item>
      </el-col>
                        <el-col :span="12">
        <el-form-item   v-if="flag=='huiyuan'"  label="Name" prop="xingming">
          <el-input v-model="ruleForm.xingming"               placeholder="Name" clearable></el-input>
        </el-form-item>
      </el-col>
                        <el-col :span="12">
        <el-form-item   v-if="flag=='huiyuan'"  label="Address" prop="shenfenzheng">
          <el-input v-model="ruleForm.shenfenzheng"               placeholder="Address" clearable></el-input>
        </el-form-item>
      </el-col>
                                                                              <el-col :span="12">
        <el-form-item   v-if="flag=='yuangong'"  label="Staff号" prop="yuangonghao">
          <el-input v-model="ruleForm.yuangonghao" readonly              placeholder="Staff号" clearable></el-input>
        </el-form-item>
      </el-col>
                                          <el-col :span="12">
        <el-form-item   v-if="flag=='yuangong'"  label="Phone" prop="shouji">
          <el-input v-model="ruleForm.shouji"               placeholder="Phone" clearable></el-input>
        </el-form-item>
      </el-col>
                        <el-col :span="12">
        <el-form-item   v-if="flag=='yuangong'"  label="Email" prop="youxiang">
          <el-input v-model="ruleForm.youxiang"               placeholder="Email" clearable></el-input>
        </el-form-item>
      </el-col>
                        <el-col :span="12">
        <el-form-item   v-if="flag=='yuangong'"  label="Name" prop="xingming">
          <el-input v-model="ruleForm.xingming"               placeholder="Name" clearable></el-input>
        </el-form-item>
      </el-col>
                        <el-col :span="12">
        <el-form-item   v-if="flag=='yuangong'"  label="Address" prop="shenfenzheng">
          <el-input v-model="ruleForm.shenfenzheng"               placeholder="Address" clearable></el-input>
        </el-form-item>
      </el-col>
                                                                                                                                          <el-col :span="12">
        <el-form-item   v-if="flag=='defaultuser'"  label="Username" prop="username">
          <el-input v-model="ruleForm.username"               placeholder="Username" clearable></el-input>
        </el-form-item>
      </el-col>
                                          <el-col :span="12">
        <el-form-item   v-if="flag=='defaultuser'"  label="Name" prop="name">
          <el-input v-model="ruleForm.name"               placeholder="Name" clearable></el-input>
        </el-form-item>
      </el-col>
                        <el-col :span="12">
        <el-form-item v-if="flag=='defaultuser'"  label="性别" prop="sex">
          <el-select v-model="ruleForm.sex" placeholder="请Option性别">
            <el-option
                v-for="(item,index) in defaultusersexOptions"
                v-bind:key="index"
                :label="item"
                :value="item">
            </el-option>
          </el-select>
        </el-form-item>
      </el-col>
                        <el-col :span="12">
        <el-form-item   v-if="flag=='defaultuser'"  label="年龄" prop="age">
          <el-input v-model="ruleForm.age"               placeholder="年龄" clearable></el-input>
        </el-form-item>
      </el-col>
                        <el-col :span="12">
        <el-form-item   v-if="flag=='defaultuser'"  label="电话" prop="phone">
          <el-input v-model="ruleForm.phone"               placeholder="电话" clearable></el-input>
        </el-form-item>
      </el-col>
                        <el-col :span="24">  
        <el-form-item v-if="flag=='defaultuser'" label="照片" prop="picture">
          <file-upload
          tip="点击上传照片"
          action="file/upload"
          :limit="3"
          :multiple="true"
          :fileUrls="ruleForm.picture?ruleForm.picture:''"
          @change="defaultuserpictureUploadChange"
          ></file-upload>
        </el-form-item>
      </el-col>
                        <el-col :span="12">
        <el-form-item   v-if="flag=='defaultuser'"  label="Email" prop="email">
          <el-input v-model="ruleForm.email"               placeholder="Email" clearable></el-input>
        </el-form-item>
      </el-col>
                                                                                                            <el-form-item v-if="flag=='users'" label="Username" prop="username">
        <el-input v-model="ruleForm.username" 
        placeholder="Username"></el-input>
      </el-form-item>
      <el-col :span="24">
      <el-form-item>
        <el-button type="primary" @click="onUpdateHandler">修 改</el-button>
      </el-form-item>
      </el-col>
      </el-row>
    </el-form>
  </div>
</template>
<script>
// 数字，邮件，Phone，url，Address校验
import { isNumber,isIntNumer,isEmail,isMobile,isPhone,isURL,checkIdCard } from "@/utils/validate";

export default {
  data() {
    return {
      ruleForm: {},
      flag: '',
      usersFlag: false,
                                                                                                                                                                                                                                                                                                                                                                              defaultusersexOptions: [],
                                                                                                                                                    };
  },
  mounted() {
    var table = this.$storage.get("sessionTable");
    this.flag = table;
    this.$http({
      url: `${this.$storage.get("sessionTable")}/session`,
      method: "get"
    }).then(({ data }) => {
      if (data && data.code === 0) {
        this.ruleForm = data.data;
      } else {
        this.$message.error(data.msg);
      }
    });
                                                                                                                                                                                                                                                    this.defaultusersexOptions = "男,女".split(',')
                                                                                                  },
  methods: {
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        defaultuserpictureUploadChange(fileUrls) {
        this.ruleForm.picture = fileUrls;
    },
                                                                            onUpdateHandler() {
                              if((!this.ruleForm.huiyuanming)&& 'huiyuan'==this.flag){
        this.$message.error('Member Name不能为空');
        return
      }
                                                                  if((!this.ruleForm.mima)&& 'huiyuan'==this.flag){
        this.$message.error('Password不能为空');
        return
      }
                                                                  if((!this.ruleForm.shouji)&& 'huiyuan'==this.flag){
        this.$message.error('Phone不能为空');
        return
      }
                                                                                                                        if((!this.ruleForm.xingming)&& 'huiyuan'==this.flag){
        this.$message.error('Name不能为空');
        return
      }
                                                                                                                                    if( 'huiyuan' ==this.flag && this.ruleForm.money&&(!isNumber(this.ruleForm.money))){
        this.$message.error(`Balance应输入数字`);
        return
      }
                                                                                          if((!this.ruleForm.yuangonghao)&& 'yuangong'==this.flag){
        this.$message.error('Staff号不能为空');
        return
      }
                                                                  if((!this.ruleForm.mima)&& 'yuangong'==this.flag){
        this.$message.error('Password不能为空');
        return
      }
                                                                                                                                                                              if((!this.ruleForm.xingming)&& 'yuangong'==this.flag){
        this.$message.error('Name不能为空');
        return
      }
                                                                  if((!this.ruleForm.shenfenzheng)&& 'yuangong'==this.flag){
        this.$message.error('Address不能为空');
        return
      }
                                                                              if( 'yuangong' ==this.flag && this.ruleForm.money&&(!isNumber(this.ruleForm.money))){
        this.$message.error(`Balance应输入数字`);
        return
      }
                                                                                                                                                      if((!this.ruleForm.username)&& 'defaultuser'==this.flag){
        this.$message.error('Username不能为空');
        return
      }
                                                                  if((!this.ruleForm.mima)&& 'defaultuser'==this.flag){
        this.$message.error('Password不能为空');
        return
      }
                                                                                                                                                                                    if( 'defaultuser' ==this.flag && this.ruleForm.age&&(!isIntNumer(this.ruleForm.age))){
       this.$message.error(`年龄应输入整数`);
        return
      }
                                                                              if( 'defaultuser' ==this.flag && this.ruleForm.phone&&(!isPhone(this.ruleForm.phone))){
        this.$message.error(`电话应输入电话格式`);
        return
      }
                                                                                                                                    if( 'defaultuser' ==this.flag && this.ruleForm.email&&(!isEmail(this.ruleForm.email))){
        this.$message.error(`Email应输入Email格式`);
        return
      }
                                                if( 'defaultuser' ==this.flag && this.ruleForm.money&&(!isNumber(this.ruleForm.money))){
        this.$message.error(`Balance应输入数字`);
        return
      }
                                                                                                                        this.$http({
        url: `${this.$storage.get("sessionTable")}/update`,
        method: "post",
        data: this.ruleForm
      }).then(({ data }) => {
        if (data && data.code === 0) {
          this.$message({
            message: "ModifyMessage成功",
            type: "success",
            duration: 1500,
            onClose: () => {
            }
          });
        } else {
          this.$message.error(data.msg);
        }
      });
    }
  }
};
</script>
<style lang="scss" scoped>
</style>
