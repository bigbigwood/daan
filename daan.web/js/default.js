
function onReady() {
    var btnExpandAll = Ext.getCmp(IDS.btnExpandAll);
    var btnCollapseAll = Ext.getCmp(IDS.btnCollapseAll);
    var treeMenu = Ext.getCmp(IDS.treeMenu);
    var mainTabStrip = Ext.getCmp(IDS.mainTabStrip);
    var windowSourceCode = Ext.getCmp(IDS.windowSourceCode);

    // 点击全部展开按钮
    btnExpandAll.on('click', function () {
        treeMenu.expandAll();
    });

    // 点击全部折叠按钮
    btnCollapseAll.on('click', function () {
        treeMenu.collapseAll();
    });


    // 点击树节点
    treeMenu.on('click', function (node, event) {
        if (node.isLeaf()) {
            // 阻止事件传播
            event.stopEvent();

            var href = node.attributes.href;

            // 修改地址栏
          //  window.location.hash = '#' + href;

            // 新增Tab节点
            addExampleTab(node);
        }
    });

//    //添加默认首页
//    var node = {
//        attributes: {
//            href: "/welcome.aspx"
//        },
//        text: "欢迎",
//        id: "welcome"
//    };
//    window.parent.addExampleTab.apply(window.parent, [node]);


    // 动态添加一个标签页
    function addExampleTab(node) {
        var href = node.attributes.href;

//        // 动态创建按钮
//        var sourcecodeButton = new Ext.Button({
//            text: "源代码",
//            type: "button",
//            cls: "x-btn-text-icon",
//            icon: "./res.axd?icon=PageWhiteCode",
//            listeners: {
//                click: function (button, e) {
//                    windowSourceCode.box_show('./source.aspx?files=' + href, '源代码');
//                    e.stopEvent();
//                }
//            }
//        });

//        var openNewWindowButton = new Ext.Button({
//            text: '新标签页中打开',
//            type: "button",
//            cls: "x-btn-text-icon",
//            icon: "./res.axd?icon=TabGo",
//            listeners: {
//                click: function (button, e) {
//                    window.open(href, "_blank");
//                    e.stopEvent();
//                }
//            }
//        });

//        var refreshButton = new Ext.Button({
//            text: '刷新',
//            type: "button",
//            cls: "x-btn-text-icon",
//            icon: "./res.axd?icon=Reload",
//            listeners: {
//                click: function (button, e) {
//                    // 注意：button.ownerCt 是工具栏，button.ownerCt.ownerCt 就是当前激活的标签页。
//                    Ext.DomQuery.selectNode('iframe', button.ownerCt.ownerCt.getEl().dom).contentWindow.location.reload(); //.replace(href);
//                    e.stopEvent();
//                }
//            }
//        });

        // 动态添加一个带工具栏的标签页
        var tabId = 'dynamic_added_tab' + node.id.replace('__', '-');
        var currentTab = mainTabStrip.getTab(tabId);
        if (!currentTab) {
            mainTabStrip.addTab({
                'id': tabId,
                'url': href,
                'title': node.text,
                'closable': true,
                'bodyStyle': 'padding:0px;',
                'iconCls': 'icon_' + href.replace(/[^.]+\./, '')
//                ,
//                'tbar': new Ext.Toolbar({
//                    items: ['->', sourcecodeButton, '-', refreshButton, '-', openNewWindowButton]
//                })
            });
        } else {
            mainTabStrip.setActiveTab(currentTab);
        }
    }

    mainTabStrip.on('tabchange', function (tabStrip, tab) {
        if (tab.url) {
            //window.location.href = '#' + tab.url;
           // window.location.hash = '#' + tab.url;
        } else {
            window.location.hash = '#';
        }
    });

    var HASH = window.location.hash.substr(1);
    var FOUND = false;

    (function (node) {
        var i, currentNode, nodes, path;
        if (!FOUND && node.hasChildNodes()) {
            nodes = node.childNodes;
            for (i = 0; i < nodes.length; i++) {
                currentNode = nodes[i];
                if (currentNode.isLeaf()) {
                    if (currentNode.attributes.href === HASH) {
                        path = currentNode.getPath();
                        treeMenu.expandPath(path); //node.expand();
                        treeMenu.selectPath(path); // currentNode.select();
                        addExampleTab(currentNode);
                        FOUND = true;
                        return;
                    }
                } else {
                    arguments.callee(currentNode);
                }
            }
        }
    })(treeMenu.getRootNode());



    window.addExampleTab = addExampleTab;
}
