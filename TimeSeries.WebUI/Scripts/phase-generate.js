Array.prototype.max = function () {
    return Math.max.apply(Math, this);
};

Array.prototype.min = function () {
    return Math.min.apply(Math, this);
};

var chart = c3.generate({
    bindto: '#chart',
    data: {
        x: 'x',
        columns: [],
        type: 'scatter'
    }
});
chart.zoom.enable;
function load2d(data) {
    var vector1 = new Array();
    var vector2 = new Array();
    vector1.push("x");
    vector2.push("2d");
    $.each(data.x, function (i, x) {
        vector1.push(x);
    });
    $.each(data.y, function (i, y) {
        vector2.push(y);
    });
    chart.load({
        columns: [vector1, vector2]
    });
}

function show(data) {
    var descr = '<h4>Данные временного ряда ' + data.Name + ':</h4>';
    descr += '<p>' + data.Data + '</p>';

    $('#serieInfo').html(descr);

    load2d(data.PhaseTimeSerie);
    load3d(data.PhaseTimeSerie);
}


function createTextCanvas(text, color, font, size) {
    size = size || 24;
    var canvas = document.createElement('canvas');
    var ctx = canvas.getContext('2d');
    var fontStr = (size + 'px ') + (font || 'Arial');
    ctx.font = fontStr;
    var w = ctx.measureText(text).width;
    var h = Math.ceil(size);
    canvas.width = w;
    canvas.height = h;
    ctx.font = fontStr;
    ctx.fillStyle = color || 'black';
    ctx.fillText(text, 0, Math.ceil(size * 0.8));
    return canvas;
}

function createText2D(text, color, font, size, segW, segH) {
    var canvas = createTextCanvas(text, color, font, size);
    var plane = new THREE.PlaneGeometry(canvas.width, canvas.height, segW, segH);
    var tex = new THREE.Texture(canvas);
    tex.needsUpdate = true;
    var planeMat = new THREE.MeshBasicMaterial({
        map: tex, color: 0xffffff, transparent: true
    });
    var mesh = new THREE.Mesh(plane, planeMat);
    mesh.scale.set(0.25, 0.25, 0.25);
    mesh.doubleSided = true;
    return mesh;
}

function load3d(data) {
    var renderer = new THREE.WebGLRenderer({ antialias: true });
    var elem = $('#chart3d');
    var w = elem.width();
    var h = elem.height();
    renderer.setSize(w, h);
    elem.html('');
    elem.append(renderer.domElement);

    renderer.setClearColor(0xFFFFFF, 1.0);

    var dataXMax = Math.abs(data.x.min()) > Math.abs(data.x.max()) ? data.x.min() : data.x.max();
    var dataYMax = Math.abs(data.y.min()) > Math.abs(data.y.max()) ? data.y.min() : data.y.max();
    var dataZMax = Math.abs(data.z.min()) > Math.abs(data.z.max()) ? data.z.min() : data.z.max();

    var camera = new THREE.PerspectiveCamera(45, w / h, 1, 10000);
    camera.position.z = dataZMax;
    camera.position.x = dataXMax;
    camera.position.y = 0;

    var scene = new THREE.Scene();
    scene.fog = new THREE.FogExp2(0xFFFFFF, 0.0035);

    var scatterPlot = new THREE.Object3D();
    scene.add(scatterPlot);

    // scatterPlot.rotation.y = 0.5;

    function v(x, y, z) { return new THREE.Vector3(x, y, z); }

    var lineMat = new THREE.LineBasicMaterial({ color: 0x808080, lineWidth: 1 });

    var dataXMin = data.x.min() > 0 ? 0 : data.x.min();
    var dataYMin = data.y.min() > 0 ? 0 : data.y.min();
    var dataZMin = data.z.min() > 0 ? 0 : data.z.min();

    var lineGeo = new Array();
    lineGeo[0] = new THREE.Geometry();
    lineGeo[0].vertices.push(
      v(dataXMin, 0, 0), v(data.x.max(), 0, 0)
    );

    lineGeo[1] = new THREE.Geometry();
    lineGeo[1].vertices.push(
      v(0, dataYMin, 0), v(0, data.y.max(), 0)
    );

    lineGeo[2] = new THREE.Geometry();
    lineGeo[2].vertices.push(
      v(0, 0, dataZMin), v(0, 0, data.z.max())
    );

    for (var i = 0; i < lineGeo.length; i++) {
        var line = new THREE.Line(lineGeo[i], lineMat);
        line.type = THREE.Lines;
        scatterPlot.add(line);
    }

    var titleX = createText2D('-X');
    titleX.position.x = -60;
    scatterPlot.add(titleX);

    var titleX = createText2D('X');
    titleX.position.x = 60;
    scatterPlot.add(titleX);

    var titleX = createText2D('-Y');
    titleX.position.y = -60;
    scatterPlot.add(titleX);

    var titleX = createText2D('Y');
    titleX.position.y = 60;
    scatterPlot.add(titleX);

    var titleX = createText2D('-Z');
    titleX.position.z = -60;
    scatterPlot.add(titleX);

    var titleX = createText2D('Z');
    titleX.position.z = 60;
    scatterPlot.add(titleX);

    var pointSize = 0.25;
    if (Math.max(dataXMax, dataYMax, dataZMax) > 1) {
        pointSize = 1.25;
    }

    var mat = new THREE.PointCloudMaterial({ vertexColors: true, size: pointSize });

    var pointCount = data.x.length;
    var pointGeo = new THREE.Geometry();
    for (var i = 0; i < pointCount; i++) {
        var x = data.x[i];
        var y = data.y[i];
        var z = data.z[i];
        pointGeo.vertices.push(new THREE.Vector3(x, y, z));
        pointGeo.vertices[i].angle = Math.atan2(z, x);
        pointGeo.vertices[i].radius = Math.sqrt(x * x + z * z);
        pointGeo.vertices[i].speed = (z / 100) * (x / 100);
        pointGeo.colors.push(new THREE.Color("rgb(31, 119, 181)"));
    }
    var points = new THREE.PointCloud(pointGeo, mat);
    scatterPlot.add(points);

    renderer.render(scene, camera);

    var paused = false;
    var last = new Date().getTime();
    var down = false;
    var sx = 0, sy = 0;
    window.onmousedown = function (ev) {
        down = true; sx = ev.clientX; sy = ev.clientY;
    };
    window.onmouseup = function () { down = false; };
    window.onmousemove = function (ev) {
        if (down) {
            var dx = ev.clientX - sx;
            var dy = ev.clientY - sy;
            scatterPlot.rotation.y += dx * 0.01;
            camera.position.y += dy;
            sx += dx;
            sy += dy;
        }
    }

    function animate(t) {
        if (!paused) {
            last = t;

            renderer.clear();
            camera.lookAt(scene.position);
            renderer.render(scene, camera);
        }
        window.requestAnimationFrame(animate, renderer.domElement);
    };
    animate(new Date().getTime());
    onmessage = function (ev) {
        paused = (ev.data == 'pause');
    };
}