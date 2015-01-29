define('jstree-map-js', ['jquery', 'jstree'], function ($, jstree) {
    'use strict';
    function pick(collection, keys) {
        for (var i = 0; i < collection.length; i++) {
            var obj = collection[i];
            var copy = {};
            $.each(keys, function (i, key) {
                if (key in obj) {
                    copy[key] = obj[key];
                    if ($.isArray(copy[key])) {
                        if (copy[key].length == 0) {
                            delete copy[key];
                        }
                        else {
                            pick(copy[key], keys);
                        }
                    }
                }
            });
            collection[i] = copy;
        }
    }
    function find(nodes, data) {
        for (var i = 0; i < nodes.length; i++) {
            if (nodes[i].data.content == data) {
                return nodes[i].id;
            }
            if (nodes[i].children) {
                var found = find(nodes[i].children, data);
                if (found != null) {
                    return found;
                }
            }
        }
        return null;
    }

    var $treeControlLeft = null;
    var $treeControlRight = null;
    var $treeLeft = null;
    var $treeRight = null;

    function initLeftTree(treeControl, data) {
        $treeControlLeft = treeControl;
        $treeControlLeft.jstree({
            'core': {
                'themes': {
                    'name': 'proton',
                    'responsive': true
                },
                'multiple': false,
                'check_callback': true,
                'data': data || []
            },
            'plugins': ['unique']
        });
        $treeLeft = $treeControlLeft.jstree(true);
    }

    function initRightTree (treeControl, data) {
        $treeControlRight = treeControl;
        $treeControlRight.jstree({
            'core': {
                'themes': {
                    'name': 'proton',
                    'responsive': true
                },
                'data': data || []
            },
            'checkbox': {
                'three_state': false,
                'visible': false
            },
            'plugins': ['unique', 'checkbox']
        });
        $treeRight = $treeControlRight.jstree(true);
    }

    function bindTreeEvents() {
        $treeControlLeft.on('select_node.jstree', function (e, data) {
            var node = data.instance.get_node(data.selected);
            $treeRight.deselect_all(true);
            if (node.data.elements != null) {
                var data = $treeRight.get_json('#', { 'no_state': true });
                $.each(node.data.elements, function (i, v) {
                    var id = find(data, v);
                    $treeRight.select_node(id, true);
                });
            }
        });
        $treeControlLeft.on('deselect_node.jstree', function (e, data) {
            $treeRight.deselect_all(true);
        });
        $treeControlRight.on('changed.jstree', function (e, data) {
            var node = $treeLeft.get_node($treeLeft.get_selected());
            if (node) {
                var selected = $.map($treeRight.get_selected(), function (v, i) {
                    return $treeRight.get_node(v).data.content;
                });
                node.data.elements = selected;
            }
        });
    }

    var App = {
        init: function (treeControlLeft, treeControlRight, leftJson, rightJson) {
            initLeftTree(treeControlLeft, leftJson);
            initRightTree(treeControlRight, rightJson);
            bindTreeEvents();
        },
        data: function() {
            var json_data = $treeLeft.get_json('#', { 'no_state': true });
            pick(json_data, ['id', 'data', 'children']);
            return json_data;
        },
        tree: function () {
            return $treeLeft;
        }
    };
    return App;
});