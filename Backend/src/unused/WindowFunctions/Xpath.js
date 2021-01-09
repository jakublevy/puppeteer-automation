function getPath(node, path) {
        path = path || [];
        if(node.parentNode)
            path = getPath(node.parentNode, path);


        if(node.previousSibling) {
            var count = 1;
            let sibling = node.previousSibling
            do {
                if(sibling.nodeType === 1 && sibling.nodeName === node.nodeName)
                    count++;

                sibling = sibling.previousSibling;
            } while(sibling);

            if(count === 1)
                count = null;

        }
        else if(node.nextSibling) {
            let sibling = node.nextSibling;
            do {
                if(sibling.nodeType === 1 && sibling.nodeName === node.nodeName) {
                    count = 1;
                    sibling = null;
                } else {
                    count = null;
                    sibling = sibling.previousSibling;
                }
            } while(sibling);
        }

        if(node.nodeType === 1) {
            let nodeInfo = { tagName: node.nodeName.toLowerCase() }
            if (node.id)
                nodeInfo.id = node.id

            if (node.classList.length > 0)
                nodeInfo.class = Array.from(node.classList)

            if(node.children.length === 0 && node.textContent.length > 0)
                nodeInfo.text = node.textContent

            if(node.value)
                nodeInfo.value = node.value

            path.push(nodeInfo)
        }

        return path;
    }

//https://stackoverflow.com/questions/5706837/get-unique-selector-of-element-in-jquery
