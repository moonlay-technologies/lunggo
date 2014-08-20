# Linkify

[![Node Dependencies](https://david-dm.org/SoapBox/jQuery-linkify/dev-status.png)](https://david-dm.org/SoapBox/jQuery-linkify#info=devDependencies&view=table)

__Download 1.1__
- [Minified](https://github.com/SoapBox/jQuery-linkify/blob/master/dist/jquery.linkify.min.js)
- [Source](https://github.com/SoapBox/jQuery-linkify/blob/master/dist/jquery.linkify.js)

__Jump to__
- [Demo](#demo)
- [Installing](#installing)
  - [Basic](#basic)
  - [Bower](#bower)
- [Examples](#examples)
  - [Basic Usage](#basic-usage)
  - [Usage via HTML attributes](#usage-via-html-attributes)
- [Options](#options)
- [Building and Development Tasks](#building-and-development-tasks)
  - [Setup](#setup)
  - [Development](#development)
- [Authors](#authors)

Linkify is a jQuery plugin for finding URLs in plain-text and converting them to HTML links. It works with all valid URLs and email addresses.

## Demo
[Launch demo](http://soapbox.github.io/jQuery-linkify/)

## Installing

### Basic
Just download [jquery.linkify.min.js](https://github.com/HitSend/jQuery-linkify/blob/master/dist/jquery.linkify.min.js) from this repo's `dist` folder and include it on your web page with `<script>` tag, along with jQuery:

```html
<script src="//ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js"></script>
<script src="js/jquery.linkify.min.js"></script>
```

### Bower
Run `bower install jQuery-linkify` from your project's root folder.


## Examples

### Basic Usage

To detect links within any set of elements, just call `$(selector).linkify()` on document load.

#### Code

```html
<p id="paragraph1">Check out this link to http://google.com</p>
<p id="paragraph2">You can also email support@example.com to view more.</p>

<script src="//ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js"></script>
<script src="js/jquery.linkify.min.js"></script>
```

```javascript
$(window).on('load', function () {
  $('p').linkify();
});
```

#### Output

``` html
<p id="paragraph1">
  Check out this link to
  <a href="http://google.com" class="linkified" target="_blank">
    http://google.com
  </a>
</p>
<p id="paragraph2">
  You can also email
  <a href="mailto:support@example.com" class="linkified">
    support@example.com
  </a>
  to view more.
</p>
```

### Usage via HTML attributes

Linkify also provides a DOM data- API. The following code will find links in the `#linkify-example` paragraph element:

```html
<p id="linkify-example" data-linkify="this">
  Lorem ipsum dolor sit amet, consectetur adipisicing
  elit, sed do eiusmod tempor incididunt ut labore et
  dolore magna aliqua.
</p>
```

Pass in a selector instead of this to linkify every element with that selector. The example below linkifies every paragraph and `.plain-text` element in the bodytag:

```html
<body data-linkify="p, .plain-text">
  ...
</body>
```

## Options

Linkify is applied with the following default options. Below is a description of each.

```javascript
$('selector').linkify({
  tagName: 'a',
  target: '_blank',
  newLine: '\n',
  linkClass: null,
  linkAttributes: null
});
```

<table>
	<thead>
		<tr>
			<th>Option</th>
			<th>Type</th>
			<th>Default</th>
			<th>Description</th>
			<th>Data Attribute</th>
		</tr>
	</thead>
	<tbody>
		<tr>
			<td>tagName</td>
			<td>String</td>
			<td><code>"a"</code></td>
			<td>
				The tag that should be used to wrap each URL. This is
				useful for cases where <code>a</code> tags might be
				innapropriate, or might syntax problems (e.g., finding
				URLs inside an <code>a</code> tag).
			</td>
			<td>
				<code class="small">data-linkify-tagname</code>
			</td>
		</tr>
		<tr>
			<td>target</td>
			<td>String</td>
			<td><code>"_blank"</code></td>
			<td><code>target</code> attribute for each linkified tag.</td>
			<td><code class="small">data-linkify-target</code></td>
		</tr>
		<tr>
			<td>newLine</td>
			<td>String</td>
			<td><code>"\n"</code></td>
			<td>
				The character to replace new lines with. Replace with
				<code>"&lt;br&gt;"</code> to space out multi-line user
				content.
			</td>
			<td><code class="small">data-linkify-newline</code></td>
		</tr>

		<tr>
			<td>linkClass</td>
			<td>String</td>
			<td><code>null</code></td>
			<td>
				The class to be added to each linkified tag. An extra <code>.linkified</code> class ensures that each link will be clickable, regardless of the value of <code>tagName</code>. Linkify won't attempt finding links in <code>.linkified</code> elements.
			</td>
			<td><code class="small">data-linkify-linkclass</code></td>
		</tr>

		<tr>
			<td>linkAttributes</td>
			<td>Object</td>
			<td><code>null</code></td>
			<td>
				HTML attributes to add to each linkified tag. In the
				following example, the <code>tabindex</code> and
				<code>rel</code> attributes will be added to each link.

<pre>
$('p').linkify({
	linkAttributes: {
		tabindex: 0,
		rel: 'nofollow'
	}
});
</pre>

			</td>
			<td>N/A</td>
		</tr>
	</tbody>
</table>

## Building and Development Tasks

### Setup

Linkify uses [Grunt](http://gruntjs.com/) for building and testing, and
[Bower](http://bower.io/) for dependency management. Both can be installed
via [npm](https://npmjs.org/) by running:

```bash
npm install -g grunt-cli
npm install -g bower
```

Once you have those, navigate into the repo's root directory and run

```bash
npm install && bower install
```

### Development

Each of these tasks can be called by running `grunt taskName` from the
repo's root folder.

1. `default`: Also available by just calling `grunt`, this task tests
the plugin code in the `src` folder for JSHint compliance and builds and
minifies it into the `dist` folder.

2. `demo`: Builds everything and launches the demo page at
[localhost:8000](http://localhost:8000/).

3. `test`: Runs the complete test suite, including JSHint and QUnit. QUnit
tests will be executed at [localhost:8001](http://localhost:8000/).


## Authors
Linkify is handcrafted with Love by [SoapBox Innovations, Inc](http://soapboxhq.com).
