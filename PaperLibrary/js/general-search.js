var selectUls=new Array();
var pageArray=new Array();//每页实际文章数
var pageSize=12;//每页可放的最多文章数
var searchkey="title";//一般搜索的searchKey(当前和全部)
var generalSearch=new Array("title","keyword","author");
function appendArticle(data,nowPage,firstSearch)
{
	$(".advanced-search .search-number p").text("共搜索到"+data.totalCnt+"条结果");
	//统计总共搜索到的数据数
	$(".search-result").empty();
	if(firstSearch)
	{
		for(var i=0;i<Math.floor(data.totalCnt/pageSize);i++)
		{
			pageArray[i]=pageSize;
		}
		pageArray[Math.floor(data.totalCnt/pageSize)]=data.totalCnt-i*pageSize;
		//计算每页实际的文章数
		var buttonNumber=0;
		//设置翻页处container长度
		if(data.totalCnt>pageSize)
		{
			$(".change-article-page button").css({"display":"block"});
			buttonNumber=2;
		}
		for(var i=0;i<Math.ceil(data.totalCnt/pageSize);i++)
		{
			if(i==0)
				$("<p>").text(i+1).addClass("now-page-number").appendTo($(".page-number-in"));
			else
				$("<p>").text(i+1).appendTo($(".page-number-in"));
		}
		$(".page-number").width(i*40);
		$(".change-article-page").width(81*buttonNumber+i*40);
	}
	for(var i=0;i<Math.ceil(pageArray[nowPage]/2);i++){
		//创建每一行container并改变颜色
		var everyLine=$("<div>").addClass("every-line").appendTo($(".search-result"));
		if(i%2)
		{
			everyLine.css({"background":"rgb(248,248,248)"});
		}
		var everyLineIn=$("<div>").addClass("every-line-in").addClass("clearfix").appendTo(everyLine);
	}
	//创建每一页的文章并添加
	for(var i=0;i<pageArray[nowPage];i++)
	{
		var lineIndex=Math.floor(i/2);
		var everyArticle=$("<div>").addClass("every-article").appendTo($(".every-line-in:eq("+lineIndex+")"));
			var h1=$("<h1>").text(data.info[i].title).attr("title",data.info[i].title).appendTo(everyArticle);
			var author=$("<div>").addClass("article-author").addClass("clearfix").appendTo(everyArticle);
				var authorLabel=$("<h2>").text("作者").appendTo(author);
				var authorauthor=$("<p>").text(data.info[i].author).appendTo(author);
			var keyword=$("<div>").addClass("article-keyword").addClass("clearfix").appendTo(everyArticle);
				var keywordLabel=$("<h2>").text("关键词").appendTo(keyword);
				var keywordk=$("<p>").text(data.info[i].keyword).appendTo(keyword);
			var summary=$("<div>").addClass("article-summary").addClass("clearfix").appendTo(everyArticle);
				var summaryLabel=$("<h2>").text("摘要").appendTo(summary);
				var summarysummary=$("<p>").text(data.info[i].summary).appendTo(summary);
			var articleTime=$("<div>").addClass("article-time").addClass("clearfix").appendTo(everyArticle);
				var timeLabel=$("<h2>").text("时间").appendTo(articleTime);
				var timetime=$("<p>").text(data.info[i].time+"年").appendTo(articleTime);
			var articleJournal=$("<div>").addClass("article-journal").addClass("clearfix").appendTo(everyArticle);
				var journalLabel=$("<h2>").text("期刊").appendTo(articleJournal);
				var journaljournal=$("<p>").text(data.info[i].journal).appendTo(articleJournal);
			var a=$("<a>").text("阅读全文").addClass("read-article").attr("href",data.info[i].link).appendTo(everyArticle);
	    ifOverflow(summarysummary);
	}
}
function getGeneral(a,b,c,firstSearch)
{
	$.ajax({ 
	    type: "POST",    
	    url: "/Ajax/querySingle.ashx",
	    dataType: "JSON",
	    async : false,
    	data:{searchKey:a,searchValue:b,currntPage:c},
	    success: function(data) {
           appendArticle(data,c,firstSearch);
           changeFooter();
	    }
	});
}
function getAdvanced(a,b,c,firstSearch)
{
	$.ajax({ 
	    type: "POST",    
	    url: "/Ajax/queryMulti.ashx",
	    dataType: "JSON",
	    async : false,
        data:{searchKey:a,time:b,currntPage:c},
	    success: function(data) {
            appendArticle(data,c,firstSearch);
            changeFooter();
		}
	});
}
function getMix(a,b,c,d,e,firstSearch)
{
	$.ajax({ 
	    type: "POST",    
	    url: "/Ajax/queryMix.ashx",
	    dataType: "JSON",
	    async : false,
        data:{searchSingleKey:a,searchSingleValue:b,searchMultiKey:c,
        time:d,currntPage:e},
	    success: function(data) {
            appendArticle(data,e,firstSearch);
            changeFooter();
		}
	});
}
//改变一般搜索的选中选项和searchkey
function changeGeneral(key)
	{
		if(key==generalSearch[0])
		{
			return;
		}
		var theIndex=0;
		for(var i=1;i<3;i++)
		{
			if(generalSearch[i]==key)
				theIndex=i;
		}
		var forChange;
		var changeText;
		forChange=generalSearch[0];
		generalSearch[0]=generalSearch[theIndex];
		generalSearch[theIndex]=forChange;
		//交换数组中2个key的位置
		changeText=$(".search-ul li:eq(0) p").text();
		$(".search-ul li:eq(0) p").text($(".search-ul li:eq("+theIndex+")").text());
		$(".search-ul li:eq("+theIndex+")").text(changeText);
		//更换输入框的提示文字
		$(".general-search-search input").val("请输入"+$(".search-ul li:eq(0) p").text());
		//交换下拉框列表里的文本
		searchkey=generalSearch[0];
	}
//根据页面长度改变footer是随着页面高度变化的还是绝对定位的
function changeFooter(){
	if($(".search-result-container").height()>$(window).height())
	{
		$(".search-result-container").css({
			"height":"auto"
		});
		$(".search-result-container .footer").css({
			"position":"static"
		});
	}
	else
	{
		$(".search-result-container").height($(window).height());
		$(".search-result-container .footer").css({
			"position":"absolute"
		});
	}
}
function ifOverflow(summarysummary)
{
		var maxSummaryLength=280;
		var startCheck=240;
		var theSummary=summarysummary.text();
		if(theSummary.length<=startCheck)
			return;
		var indexEnd=theSummary.indexOf('。',startCheck);
		if(indexEnd==-1)
			summarysummary.text(theSummary.substring(0,260)+"……");
		else
			summarysummary.text(theSummary.substring(0,indexEnd+1)+"……");
		
}
$(function(){
	var timer;
	var liHeight=32;
	var liSize=6;
	var ulObject=new Array();
	initialPage();
	//高级搜索页和搜索结果页创建下拉框选项
	if(!$(".container").hasClass("general-search-container"))
		getAndCreate();
	function initialPage()
	{
		$(".general-search-container").css({
			"height":$(window).height()
		});
		$(".advanced-search-container").css({
			"height":$(window).height()
		});
		$(".footer").css({
			"display":"block",
			"left":($(".container").width()-$(".footer").width())/2
		});
	}
	$(window).resize(function(){
		initialPage();
	})
	//清空搜索的事件
	$(".clear-search").click(function(){
		for(var i=0;i<$(".advanced-search-search ul").length;i++)
		{
			var nowUl=$(".advanced-search-search ul:eq("+i+")");
			if(!nowUl.hasClass("second-title")||nowUl.parents("div").hasClass("select-time"))
			{
				if(nowUl.hasClass("search-ul"))
				{
					if(generalSearch[0]=="title")
					$(".general-search-search input").val("请输入文章标题");
					changeGeneral("title");
				}
				else{
					nowUl.find("li:eq(1)").click();
				}
			}		
		}
	})
	//普通搜素input的事件
	$(".general-search-search input").focus(function(){
		if($(this).val()=="请输入文章标题"||$(this).val()=="请输入关键字"||$(this).val()=="请输入作者")
			$(this).val("");
	})
	$(".general-search-search input").blur(function(){
		if($(this).val()=="")
			$(this).val("请输入"+$(".search-ul li:eq(0) p").text());
	})
	//以下为普通搜素（单条件搜索）的事件
	$(".search-ul li").click(function(){
		hideOption($(".search-ul"),"blue",liHeight,false);
		var theIndex=$(".search-ul li").index(this);
		if(theIndex)
		{
			changeGeneral(generalSearch[theIndex]);
		}		
	})
	//点击搜索和按enter键时判断搜索类型并传送数据
	$(".start-search").click(function(){
		judgeSearchtype();
	})
	$(".general-search-search input").keypress(function(event){  
	    var keycode = (event.keyCode ? event.keyCode : event.which);  
	    if(keycode == '13'){  
	    	judgeSearchtype();  
	    }  
	});
	//以下为高级搜索的事件
	$(".advanced-search-search ul").hover(function(){
		var ulHeight;
		if($(this).hasClass("search-ul"))//如果是一般搜索的ul
			ulHeight=3*liHeight;
		else if($(this)[0].liNumber)
			ulHeight=$(this)[0].liNumber*liHeight;
		else
		{
			for(var i=0;i<ulObject.length;i++)
				if($(this).attr("id")==ulObject[i].id)
				{
					ulHeight=ulObject[i].liNumber*liHeight;
					break;
				}
		}
		if(ulHeight>(liSize+1)*liHeight||$(this).parents("div").hasClass("select-time"))
		{
			ulHeight=(liSize+1)*liHeight;
			showOption($(this),"blue",ulHeight,true);
		}
		else{
			showOption($(this),"blue",ulHeight,false);
		}
	},function(){
		if(!$(this).hasClass("oppo-style"))
			hideOption($(this),"white",liHeight);
		else
			hideOption($(this),"blue",liHeight);
	})
	//选择一级菜单
	$(".select-level1").delegate(".select-option","click",function(){
		if(!$(this).hasClass("oppo-style"))
			hideOption($(this).parents("ul"),"white",liHeight);
		else
			hideOption($(this).parents("ul"),"blue",liHeight);
     	for(var i=0;i<selectUls.length;i++)
		{
			if(selectUls[i].name==$(this).parents("ul").attr("id"))
			{
				if($(this).hasClass("select-all"))
				{
					$(this).parents("ul").find(".select-title p").text(selectUls[i].firstT);
					selectUls[i].setSelected(0);
					return;
				}
				selectUls[i].setSelected($(this).attr("id"));
			}
		}
		$(this).parents("ul").find(".select-title p").text($(this).text());
	});  
	//选择二级菜单的一级标题
	$(".select-level2 .first-title .select-option").click(function(){
		hideOption($(this).parents("ul"),"blue",liHeight);
		$(this).parents(".select-level2").find(".second-title").css({"display":"none"});
		if($(this).parents("ul").attr("id")=="documentType")
		{
			selectUls[0].setSelected(0);
			if($(this).hasClass("select-all"))
			{
				$(this).parents("ul").find(".select-title p").text("文献类型");				
				return;
			}
		}else{
			selectUls[1].setSelected(0);
			if($(this).hasClass("select-all")){
				$(this).parents("ul").find(".select-title p").text("非市场价值类型");
				return;
			}
		}
		var optionIndex=$(this).parents(".select-level2").find(".first-title .select-option").index(this);
		$(this).parents("ul").find(".select-title p").text($(this).text());
		var secondUl=$(this).parents(".select-level2").find(".second-title:eq("+(optionIndex-1)+")");
		secondUl.css({"display":"block"});
		// if($(this).parent(".first-title").attr("id")=="documentType")
		// 	selectUls[0].setSelected(secondUl.find(".select-title").attr("id"));	
		// else
		// 	selectUls[1].setSelected(secondUl.find(".select-title").attr("id"));	
	})
	//选择二级菜单的二级标题
	$(".select-level2 .second-title .select-option").click(function(){
		hideOption($(this).parents(".second-title"),"white",liHeight);
		if($(this).hasClass("select-all"))
			return;
		for(var i=0;i<selectUls.length;i++)
		{
			if(selectUls[i].name==$(this).parents(".select-level2").find(".first-title").attr("id"))
				selectUls[i].setSelected($(this).attr("id"));
		}
		var changeLi;
		var changeId;
		changeLi=$(this).text();
		$(this).text($(this).parents(".second-title").find(".select-title p").text());
		$(this).parents(".second-title").find(".select-title p").text(changeLi);
		changeId=$(this).attr("id");
		$(this).attr("id",$(this).parents(".second-title").find(".select-title").attr("id"));
		$(this).parents(".second-title").find(".select-title").attr("id",changeId);
	})
	//选择年份
	$(".select-time .select-option").click(function(){
		hideOption($(this).parents("ul"),"white",liHeight);
		$(this).parents("ul").find(".select-title p").text($(this).text());
	})
	$(".search-result-container .title-english").click(function(){
		location.href="/";
	})
	$(".search-result-container .title-chinese").click(function(){
		location.href="/";
	})
	//判断查询类型
	function judgeSearchtype()
	{
		//判断一般搜索框是否为空
		if($(".general-search-search input").val()!="请输入文章标题"&&
			$(".general-search-search input").val()!="请输入作者"&&
			$(".general-search-search input").val()!="请输入关键字"&&
			$(".general-search-search input").val()!="")
		{
			var chooseMulti=false;
			for(var i=0;i<selectUls.length;i++)
			{
				if(selectUls[i].selectedId)
					chooseMulti=true;
			}
			if($(".select-time .first-title .select-title p").text()!=2000
				||$(".select-time .second-title .select-title p").text()!=2017)
				chooseMulti=true;
			if(chooseMulti)
				jumpAndPost("mix");
			else
				jumpAndPost("single");
		}
		else
			jumpAndPost("multi");
	}
	//根据查询类型跳转
	function jumpAndPost(searchtype)
	{
		if(searchtype=="single")
		{
			var searchContent=$(".general-search-search input").val();
			var url="search-result.html?searchKey="+escape(searchkey);
			url+="&searchValue="+escape(searchContent);
			url+="&currntPage="+escape("0");
			window.location.href=url;
		}
		else if(searchtype=="multi")
		{
			var advancedKey=getAdvancedKey();
			var time=getTime();
			postMultiData(advancedKey,time);
		}
		else if(searchtype=="mix"){
			var searchContent=$(".general-search-search input").val();
			var advancedKey=getAdvancedKey();
			var time=getTime();
			postMixData(searchkey,searchContent,advancedKey,time);
		}
	}
	//高级搜索传送数据
	function postMultiData(advancedKey,time){
		var url="search-result.html?searchKey="+escape(advancedKey);
		url+="&time="+escape(time);
		url+="&currntPage="+escape("0");
		window.location.href=url;
	}
	//混合搜索传送数据
	function postMixData(singlekey,singlevalue,advancedKey,time){
		var url="search-result.html?searchSingleKey="+escape(singlekey);
		url+="&searchSingleValue="+escape(singlevalue);
		url+="&searchMultiKey="+escape(advancedKey);
		url+="&time="+escape(time);
		url+="&currntPage="+escape("0");
		window.location.href=url;
	}
	//获取页面上选中的时间
	function getTime()
	{
		var time="";
		var startTime=$(".select-time .first-title .select-title p").text();
		var lastTime=$(".select-time .second-title .select-title p").text();
		if(startTime-0>lastTime-0)
		{
			var changeTime=startTime;
			startTime=lastTime;
			lastTime=changeTime;
		}
		//如果前者比后者小交换位置
		time+=startTime+";";
		time+=lastTime;
		return time;
	}
	//获取页面选择的搜索条件
	function getAdvancedKey()
	{
		var advancedKey="";
		for(var i=0;i<selectUls.length;i++)
		{
			if(selectUls[i].selectedId)
			{
				advancedKey+=selectUls[i].selectedId;
				advancedKey+=";";
			}
		}
		if(!advancedKey)
			advancedKey="-1";
		else
			advancedKey=advancedKey.substring(0,advancedKey.length-1);
		return advancedKey;
	}
	function showOption(obj,changeTo,ulHeight,pleaseScroll)
	{
		obj.addClass("search-ul-hover");
		if(changeTo=="white")
			obj.find(".select-title img").attr("src","images/select-white.png");
		else if(changeTo=="blue")
			obj.find(".select-title img").attr("src","images/select-blue.png");
		//if(!$(".advanced-search-search ul").is(":animated"))
			timer=setTimeout(function(){
				obj.addClass("search-ul-hover");
				obj.stop()
				.animate({"height":ulHeight,"borderBottomLeftRadius":0,"borderBottomRightRadius":0},200,scroll);
				$(".advanced-search-search ul").height(32);
			},50);
		function scroll()
		{
			//时间下拉框加载完后显示滚动条
			if(pleaseScroll){
				//obj.css({"overflowY":"scroll"});
				obj.find(".options").mCustomScrollbar({
	    			theme:"light",
	    			asix:"y"
				});
			}
		}			
	}
	function hideOption(obj,changeTo,ulHeight)
	{
		clearTimeout(timer);
		if(obj.parents("div").hasClass("select-time"))
		{
			obj.scrollTop(0);
			obj.css({"overflowY":"hidden"});
		}
		obj.stop().animate({"height":ulHeight},200,function(){
			obj.removeClass("search-ul-hover");
			if(obj.hasClass("second-title"))
				obj.css({"borderBottomRightRadius":16});
			else if(obj.hasClass("first-title")||obj.hasClass("search-ul")){
				if(!obj.parent("div").parent("div").hasClass("select-time"))
					obj.css({"borderBottomLeftRadius":16});
			}
			else
				obj.css({"borderBottomLeftRadius":16,"borderBottomRightRadius":16});
			if(changeTo=="white")
				obj.find(".select-title img").attr("src","images/select-white.png");
			else if(changeTo=="blue")
				obj.find(".select-title img").attr("src","images/select-blue.png");
		});
	}
	function getAndCreate()
	{
		setEverySelect();
		initializeSelect();
		var json=getOptionData();
		createSelectTime();
		for(var i=0;i<json.length;i++)
		{
			var selectTitle=json[i].firstType;
			if(selectTitle=="文献综述"||selectTitle=="一级评估"||selectTitle=="二级评估")
			{
				selectUls[0].getType(json[i]);
				continue;
			}
			for(var j=0;j<selectUls.length;j++)
			{
				if(selectTitle==selectUls[j].firstT)
				{
					selectUls[j].getType(json[i].secondType);
					selectUls[j].createOption();
				}
			}
		}
		selectUls[0].createOption();
	}
	function createSelectTime(){
		var nowYear=new Date().getFullYear();
		for(var i=0;i<2;i++)
		{
			var timeUl=$(".select-time ul:eq("+i+")");
			var optionContainer=$("<div>").addClass("options").height(liSize*liHeight).appendTo(timeUl);
		}
		for(var j=0;j<nowYear-2000+1;j++)
			$("<li>").text(j+2000).addClass("select-option").appendTo(".select-time .options:eq(0)");
		for(var j=nowYear-2000;j>=0;j--)
			$("<li>").text(j+2000).addClass("select-option").appendTo(".select-time .options:eq(1)");
		
	}
	function getOptionData()
	{
		var json=[];
		$.ajax({ 
	        type: "GET",    
	        url: "/Ajax/getOptions.ashx",
	        dataType: "JSON",
	        async : false,
	        success: function(data) {
                json=data;
	        }
	    });
	    return json;
	}
	function initializeSelect()
	{
		selectUls[0]=new everySelect("documentType","文献类型",true);
		selectUls[1]=new everySelect("valueType","非市场价值类型",false);
		selectUls[2]=new everySelect("place","地区",false);
		selectUls[3]=new everySelect("ecoSystem","生态系统类型",false);
		selectUls[4]=new everySelect("ecoService","生态系统服务类型",false);
		selectUls[5]=new everySelect("model","方法",false);
	}
	function everySelect(id,firstType,twolevels)
	{
		this.name=id;
		this.selectedId=0;
		this.obj=$(document.getElementById(id));
		this.twolevels=twolevels;
		//获取该id对象并转化为jquery对象
		this.firstT=firstType;
		this.secondT=new Array();
		/*this.serverIds=[];*/
	}
	function setEverySelect(){
		everySelect.prototype.setSelected=function(number)
		{
			this.selectedId=number;
		}
		everySelect.prototype.getType=function(jsonObj){
			if(!this.twolevels)
				this.secondT=jsonObj;
			//获得sencondType
			else
			{
				this.secondT.push(jsonObj);
				//获得json数组
			}
		}
		everySelect.prototype.createOption=function(){
			ulObject.push({"id":this.name,"liNumber":this.secondT.length+2});
			//+2 一为选择标题，二为全部选项
			//如果不是是2级菜单
			if(!this.twolevels)
			{
				var optionContainer=$("<div>").addClass("options").appendTo(this.obj);
				//secondType的数量加上“全部”选项大于限制的li长度
				if(this.secondT.length+1>liSize)
					optionContainer.height(liSize*liHeight);
				$("<li>").addClass("select-all select-option").text("全部").appendTo(optionContainer);
				//下拉框内容为全部
				for(var i=0;i<this.secondT.length;i++)
				{
					$("<li>").attr("id",this.secondT[i].id).addClass("select-option").text(this.secondT[i].name).appendTo(optionContainer);			
				}
			}else{
				$("<li>").addClass("select-all select-option").text("全部").appendTo(this.obj);
				for(var i=0;i<this.secondT.length;i++)
				{
					$("<li>").text(this.secondT[i].firstType).addClass("select-option").appendTo(this.obj);
					var ulObj=$("<ul>").addClass("second-title").appendTo(this.obj.parent(".select-level2"));
					//给一级菜单的父元素添加二级菜单
					console.log(this.secondT[i]);
					ulObj[0].liNumber=this.secondT[i].secondType.length+1;
					var all=$("<li>").attr("id","").addClass("select-all select-title").appendTo(ulObj);
						var div=$("<div>").addClass("clearfix").appendTo(all);
							var p=$("<p>").text("全部").appendTo(div);
							var img=$("<img>").attr("src","images/select-white.png").appendTo(div);
						//创建二级菜单的标题
					var optionContainer=$("<div>").addClass("options").appendTo(ulObj);
					if(this.secondT[i].secondType.length>liSize)
						optionContainer.height(liSize*liHeight);
					//optionContainer用来存放secondType的选项
						for(j=0;j<this.secondT[i].secondType.length;j++)
						{
							var liObj=this.secondT[i].secondType[j];
							var option=$("<li>").attr("id",liObj.id).appendTo(optionContainer);
							option.text(liObj.name).addClass("select-option");
						}
						//创建二级菜单的其它选项
				}
			}
		}
	}
})