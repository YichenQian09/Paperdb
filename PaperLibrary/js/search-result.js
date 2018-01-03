$(function(){
	var searchType;
	var Request=getData();
	//进入页面时，设置搜索栏初始状态，请求数据，添加文章
	if(searchType=="multi"){
		setInitial("multi");
		getAdvanced(Request["searchKey"],Request["time"],Request["currntPage"],true);
	}
	else if(searchType=="mix")
	{
		setInitial("mix");
		getMix(Request["searchSingleKey"],Request["searchSingleValue"],Request["searchMultiKey"],
        Request["time"],Request["currntPage"],true);
	}
	else{
		setInitial("single");
		getGeneral(Request["searchKey"],Request["searchValue"],Request["currntPage"],true);
	}
	//从地址栏中获取传过来的搜索信息
	function getData()
	{
		var url=location.search;
		var Request = new Object();
		if(url.indexOf("?")!=-1)
		{
			var str = url.substr(1);　//去掉?号
			var strs = str.split("&");//分割成类似name="xiaoming"的数组
			for(var i=0;i<strs.length;i++)
			{
			 　 Request[strs[i].split("=")[0]]=unescape(strs[ i].split("=")[1]);
			}
			//根据获取键值对的键判断是一般搜索还是高级搜索
			if(strs[1].split("=")[0]=="searchValue")
				searchType="single";
			else if(strs[1].split("=")[0]=="time")
				searchType="multi";
			else
				searchType="mix";
		}
		return Request;
	}
	function setInitial(type)
	{
		var idArray;
		//判断搜索类型得到选中的id数组
		if(type=="single")
		{
			changeGeneral(Request["searchKey"]);
			$(".general-search-search input").val(Request["searchValue"]);
			return;
		}
		else if(type=="mix")
		{
			changeGeneral(Request["searchSingleKey"]);
			$(".general-search-search input").val(Request["searchSingleValue"]);
			idArray=Request["searchMultiKey"].split(";");
		}
		else
			idArray=Request["searchKey"].split(";");
		var timeArray=Request["time"].split(";")
		//进入页面时初始化时间下拉框
		$(".select-time .first-title .select-title p").text(timeArray[0]);
		$(".select-time .second-title .select-title p").text(timeArray[1]);
		for(var i=0;i<idArray.length;i++)
		{
			var selectedLi=$(document.getElementById(idArray[i]));
			//获取应该选中的li
			if(selectedLi.parents("ul").hasClass("second-title"))
			//如果是二级菜单下的li
			{
				//以下是修改第一级菜单
				var firstLevel=selectedLi.parents(".select-level2").find(".first-title");
				var optionIndex=selectedLi.parents(".select-level2").find(".second-title").index(selectedLi.parents("ul"));
				firstLevel.find(".select-title p").text(firstLevel.find(".select-option:eq("+(optionIndex+1)+")").text());
				selectedLi.parents(".second-title").css({"display":"block"});
				//以下是修改第二级菜单
				for(var j=0;j<selectUls.length;j++)
				{
					if(selectUls[j].name==selectedLi.parents(".select-level2").find(".first-title").attr("id"))
						selectUls[j].setSelected(idArray[i]);
				}
				var changeLi;
				var changeId;
				//交换文本改变下拉框
				if(!selectedLi.hasClass("select-title")){
					changeLi=selectedLi.text();
					selectedLi.text(selectedLi.parents(".second-title").find(".select-title p").text());
					selectedLi.parents(".second-title").find(".select-title p").text(changeLi);
				}
				//交换id
				changeId=selectedLi.attr("id");
				selectedLi.attr("id",selectedLi.parents(".second-title").find(".select-title").attr("id"));
				selectedLi.parents(".second-title").find(".select-title").attr("id",changeId);
			}else{
				for(var j=0;j<selectUls.length;j++)
				{
					if(selectUls[j].name==selectedLi.parents("ul").attr("id"))
						selectUls[j].setSelected(idArray[i]);
				}
				//修改selectedUls数组
				selectedLi.parents("ul").find(".select-title p").text(selectedLi.text());
				//将下拉框选中
			}
		}
	}
	var nowPage=0;//现在是第几页数据
	//点击数字时切换页
	$(".page-number p").click(function(){
		var allPage=$(".page-number p").length;
		var nextPage=$(this).text()-1;
		if(allPage>10)
		{
			movePageNumber(allPage,nextPage,40);
			//如果有需要移动数字
		}
		$(".page-number p:eq("+nowPage+")").removeClass("now-page-number");
		$(this).addClass("now-page-number");
		nowPage=nextPage;
		//最后改变当前页
		changePage();
	})
	//点击上一页下一页时切换页
	$(".change-article-page button").click(function(){
		var plusminus=$(".change-article-page button").index(this);
		//点击上一页为0 下一页为1
		var allPage=$(".page-number p").length;
		console.log(allPage);
		var nextPage=nowPage;
		if(!plusminus&&nowPage>0)
			nextPage=nowPage-1;
		else if(plusminus&&nowPage<allPage-1)
			nextPage=nowPage+1;
		if(allPage>10)
		{
			movePageNumber(allPage,nextPage,40);
		}
		$(".page-number p:eq("+nowPage+")").removeClass("now-page-number");
		$(".page-number p:eq("+nextPage+")").addClass("now-page-number");
		nowPage=nextPage;
		changePage();
	})
	function movePageNumber(allPage,nextPage,width)
	{
		function setPosition(marginleft)
		{
			$(".page-number-in").css({
				"marginLeft":marginleft
			});
		}
		//模仿百度搜索效果，使得页面上始终10个数字
		//并且当前页尽可能居中
		if(nextPage>5&&allPage-nextPage>5)
		{
			setPosition(-(nextPage-5)*width);
		}else if(nextPage>5){
			setPosition(-(allPage-10)*width);
		}else
		{
			setPosition(0);
		}
	}
	function changePage()
	{
		if(searchType=="multi")
			getAdvanced(Request["searchKey"],Request["time"],nowPage,false);
		else if(searchType=="mix")
			getMix(Request["searchSingleKey"],Request["searchSingleValue"],Request["searchMultiKey"],
        Request["time"],nowPage,false);
		else
		getGeneral(Request["searchKey"],Request["searchValue"],nowPage,false);
	}
})