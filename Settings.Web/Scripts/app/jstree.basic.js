define('jstree-basic-js', ['jquery', 'jstree'], function ($, jstree) {
    'use strict';
    var $treeControl = null;
    var $tree = null;

    function bindTreeEvents(select, deselect) {
        $treeControl.on('select_node.jstree', function (e, data) {
            var node = data.instance.get_node(data.selected);
            select(node.data.content);
        });
        $treeControl.on('deselect_node.jstree', function (e, data) {
            var node = data.instance.get_node(data.selected);
            deselect(node.data.content);
        });
    }

    var App = {
        init: function (treeControl, treeJson) {
            $treeControl = treeControl;
            $treeControl.jstree({
                'core': {
                    "multiple": false,
                    'themes': {
                        'name': 'proton',
                        'responsive': true
                    },
                    'check_callback': true,
                    'data': treeJson || []
                }
            });
            $tree = $treeControl.jstree(true);
        },
        tree: function () {
            return $tree;
        },
        bind: function (select, deselect) {
            bindTreeEvents(select, deselect);
        }
    };
    return App;
});
